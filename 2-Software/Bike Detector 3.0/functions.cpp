//
//  functions.cpp
//  
//
//  Created by Shantanu Bobhate on 3/17/16.
//
//

#include "functions.hpp"

/*
 * Variables for CallBackFunc
 */
int mouse_x = 0, mouse_y = 0;

flags stateFlags;

void setup(const int argc, const char** argv) { 

    stateFlags.debug = false;
    stateFlags.train = false;
    stateFlags.performanceTest = false;
    stateFlags.detection = false;
    stateFlags.cameraId = -1;
    stateFlags.videoFile = ""; 
    stateFlags.cameraTest = false;

    if (argc < 2) {
        printHelp();
    } else {
        for (int ii = 1; ii < argc; ++ii) {
	    string argument = string(argv[ii]);
	    if (argument == "--debug") stateFlags.debug = true;            
            if (argument == "--train") stateFlags.train = true;
            if (argument == "--performanceTest") stateFlags.performanceTest = true;
            if (argument == "--detection") stateFlags.detection = true;
            if (argument == "--video") stateFlags.videoFile = string(argv[++ii]);
	    if (argument == "--camera") stateFlags.cameraId = atoi(argv[+ii]);
	    if (argument == "--cameraTest") {
		stateFlags.cameraTest = true;
		stateFlags.cameraId = atoi(argv[+ii]);
	    }
        }
    }
	
	cout << "Flags:" << endl;
	cout << "\tdebug " << stateFlags.debug << endl;
 	cout << "\ttrain " << stateFlags.train << endl;
	cout << "\tperformanceTest " << stateFlags.performanceTest << endl;
	cout << "\tdetection " << stateFlags.detection << endl;
	cout << "\tvideo " << stateFlags.videoFile << endl;
	cout << "\tcameraId " << stateFlags.cameraId << endl;
	cout << "\tcameraTest " << stateFlags.cameraTest << endl;
}

void printHelp()
{
    string output = string("Bike Detector 3.0 Help\n") +
    "  [--debug] # debug mode\n" +
    "  [--train] # train svm\n" +
    "  (--performanceTest | --detection) # measure performance or detect bikes\n" +
    "  (--video <video_file> | --camera <camera_id>) # frame source\n" +
    "  (--cameraTest <camera_id>) # tests the specified camera and also used for getting dimensions for ROI\n";
    printf("%s", output.c_str());
}


void extractFeatures (string folder, int numberOfImages) {
    // Vectors to hold the features for the images
    vector < vector <float> > descriptors;
    vector < vector <Point> > locations;
    
    Mat img;
    
    for (int ii = 0; ii < numberOfImages; ++ii) {
        // Convert the number to string
        ostringstream convert;
        convert << ii;
        string fileName = folder + "-" + convert.str() + ".png";
        if (stateFlags.debug) printf("Opening image: %s\n", fileName.c_str());
        img = imread("train/" + folder + "/" + fileName);
        
        // Convert to grayscale
        Mat gray;
        if(img.empty())
            printf("File %s is corrupted.\n", fileName.c_str());
        else if(img.channels() > 1)
            cvtColor(img, gray, CV_BGR2GRAY);
        else gray = img;
        
        // Extract the features
        HOGDescriptor hog(Size(32, 64), Size(8, 8), Size(4, 4), Size(4, 4), 9);
        long descriptorSize = hog.getDescriptorSize();
        printf("Descriptor Size: %ld\n", descriptorSize);
        vector <float> desc;
        vector <Point> loc;
        hog.compute(gray, desc, Size(0, 0), Size(0, 0), loc);
        descriptors.push_back(desc);
        locations.push_back(loc);
        
        if (stateFlags.debug) {
             imshow("Origin", img);
             waitKey(5);
        }
    }
    
    // Save the features
    FileStorage hogXml("files/" + folder + ".xml", FileStorage::WRITE);
    
    int row = (int) descriptors.size();
    int col;
    if (!descriptors.empty()) col = (int) descriptors[0].size();
    else col = 0;
    Mat M(row, col, CV_32F);
    
    for (int ii = 0; ii < row; ++ii) {
        memcpy( &(M.data[col*ii*sizeof(float) ]) , descriptors[ii].data(), col*sizeof(float));
    }
    
    write(hogXml, "Descriptor_of_images", M);
    
    hogXml.release();
    
    if (stateFlags.debug) destroyWindow("Origin");
}

void trainSVM () {
    // Train positve features
    FileStorage readPositiveXml("files/pos.xml", FileStorage::READ);
    Mat pMat;
    readPositiveXml["Descriptor_of_images"] >> pMat;
    int pRow, pCol;
    pRow = pMat.rows;
    pCol = pMat.cols;
    readPositiveXml.release();
    
    // Train negative features
    FileStorage readNegativeXml("files/neg.xml", FileStorage::READ);
    Mat nMat;
    readNegativeXml["Descriptor_of_images"] >> nMat;
    int nRow, nCol;
    nRow = nMat.rows;
    nCol = nMat.cols;
    readNegativeXml.release();
    
    printf("pRow: %d, pCol: %d\nnRow: %d, nCol: %d\n", pRow, pCol, nRow, nCol);
    
    // Set up the training matrix
    Mat PN_Descriptor_mtx( pRow + nRow, pCol, CV_32FC1 );
    memcpy( PN_Descriptor_mtx.data, pMat.data, sizeof(float) * pMat.cols * pMat.rows );
    int startP = sizeof(float) * pMat.cols * pMat.rows;
    memcpy(&(PN_Descriptor_mtx.data[ startP ]), nMat.data, sizeof(float) * nMat.cols * nMat.rows);
    Mat labels( pRow + nRow, 1, CV_32SC1, Scalar(-1.0) );
    labels.rowRange( 0, pRow ) = Scalar( 1.0 );
    
    // Set the SVM
    CvSVMParams params;
    params.svm_type    = CvSVM::C_SVC;
    params.kernel_type = CvSVM::LINEAR;
    params.term_crit   = cvTermCriteria(CV_TERMCRIT_ITER, 100, 1e-6);
    
    // Train the SVM
    PrimalSVM svm;
    svm.train(PN_Descriptor_mtx, labels, Mat(), Mat(), params);
    
    // Save the SVM
    svm.save( "files/trainedSVM.xml" );
}

void performanceTest (int numberOfPosTestImages, int numberOfNegTestImages) {
    PrimalSVM svm;
    svm.load ( "files/trainedSVM.xml" );

    int truePositives = 0, falseNegatives = 0;
    int trueNegatives = 0, falsePositives = 0;
    
    for (int ii = 0; ii < numberOfPosTestImages; ++ii) {
        
        stringstream ss;
        ss << ii;
        string fileName = "test/pos/pos-" + ss.str() + ".png";
        Mat input_img = imread(fileName);
        
        if (stateFlags.debug) {
            printf("Looking at file %s\n", fileName.c_str());
            imshow("Test Image", input_img);
        }        

        // Create a hog to compute the features for the region of interest
        HOGDescriptor hog(Size(32, 64), Size(8, 8), Size(4, 4), Size(4, 4), 9);
        vector<float> desc;
        vector<Point> loc;
        hog.compute(input_img, desc, Size(0,0), Size(0,0), loc);
        
        Mat fm(desc);
        Mat fm_ = fm.reshape(1, 1);
        
        // Predict if a match exists
        float result = svm.predict(fm_);
        if (result == 1) {
            printf("Correct Hypothesis\n");
            truePositives++;
        } else {
            printf("Wrong Hypothesis\n");
            falseNegatives++;
        }
    }
    
    for (int ii = 0; ii < numberOfNegTestImages; ++ii) {
        
        stringstream ss;
        ss << ii;
        string fileName = "test/neg/neg-" + ss.str() + ".png";
        Mat input_img = imread(fileName);
        
        if (stateFlags.debug) {
            printf("Looking at file %s\n", fileName.c_str());
        	imshow("Test Image", input_img);
        }
        
        // Create a hog to compute the features for the region of interest
        HOGDescriptor hog(Size(32, 64), Size(8, 8), Size(4, 4), Size(4, 4), 9);
        vector<float> desc;
        vector<Point> loc;
        hog.compute(input_img, desc, Size(0,0), Size(0,0), loc);
        
        Mat fm(desc);
        Mat fm_ = fm.reshape(1, 1);
        
        // Predict if a match exists
        float result = svm.predict(fm_);
        if (result == 1) {
            printf("Wrong Hypothesis\n");
            falsePositives++;
        } else {
            printf("Correct Hypothesis\n");
            trueNegatives++;
        }
    }
    
    double truePositiveRate = ( (double) truePositives / (double) numberOfPosTestImages);
    double falsePositiveRate = ( (double) falsePositives / (double) numberOfNegTestImages);
    double precision = ( (double) truePositives / ( (double) truePositives + (double) falsePositives));
    double accuracy = ( (double) truePositives + (double) trueNegatives) / ( (double) numberOfPosTestImages + (double) numberOfNegTestImages);

    cout << "Summary of performance test: \n"
         << "total number of positives = " << numberOfPosTestImages
         << "\ntotal number of negatives = " << numberOfNegTestImages
         << "\ntrue positves = " << truePositives
         << "\nfalse positives = " << falsePositives
         << "\ntrue negatives = " << trueNegatives
         << "\nfalse negatives = " << falseNegatives
         << "\ntrue positive rate = " << truePositiveRate
         << "\nfalse positive rate = " << falsePositiveRate
         << "\nprecision: " << precision
         << "\naccuracy: " << accuracy << endl;

    cout << "Confusion Matrix: \n"
         << "-------------\n"
         << "| " << truePositives << " |  " << falsePositives << " |\n"
         << "-------------\n"
         << "|  " << falseNegatives << " | " << trueNegatives << " |\n"
         << "-------------" << endl;
}

void findBikes (int top_left_x, int top_left_y, int roi_width, int roi_height) {
    // Setup SVM
    PrimalSVM svm;
    svm.load( "files/trainedSVM.xml" );
    vector<float> primal;
    svm.getSupportVector(primal);
    HOGDescriptor hog(Size(32, 64), Size(8, 8), Size(4, 4), Size(4, 4), 9);  

    printf("Size of primal vector: %d\n", primal.size());
    printf("HogDescriptor size: %zu\n", hog.getDescriptorSize());
    
    hog.setSVMDetector(primal);

    VideoCapture cap;
    
    if (stateFlags.cameraId != -1) {
		  cap.open(0);
    } else if (stateFlags.videoFile != "") {
        printf("Opening %s", stateFlags.videoFile.c_str());
	cap.open(stateFlags.videoFile);
    } else {
        printf("Error: No source specified for detection.\n");
    }

    if (!cap.isOpened()) printf("Error: could not open source.\n");

    Mat input_img;
	 bool prevDetectionState = false;
	 bool currentDetectionState = false;
		
	int detection_count = 0;
    // Test all the images for cars
    while (cap.read(input_img)) {
        
        // Resize image to match training samples
        resize(input_img, input_img, Size(648, 486));
        
        // Choose the region of focus
        input_img = input_img(Rect(top_left_x, top_left_y, roi_width, roi_height));
        
        // Get the dimensions for the image
        int width = input_img.cols;
        int height = input_img.rows;
                
        vector<Rect> found_locs;

        hog.detectMultiScale(input_img, found_locs, 0.0, Size(4, 4), Size(0, 0), 1.05, 2);

        // Draw detections
        vector<Rect> found_filtered;
        size_t i, j;
        for (i=0; i<found_locs.size(); i++)
        {
            Rect r = found_locs[i];
            for (j=0; j<found_locs.size(); j++)
                if (j!=i && (r & found_locs[j])==r)
                    break;
            if (j==found_locs.size())
                found_filtered.push_back(r);
        }
        
        int bikes_detected = found_filtered.size();

        for (i=0; i<found_filtered.size(); i++)
        {
            Rect r = found_filtered[i];
            r.x += cvRound(r.width*0.1);
            r.width = cvRound(r.width*0.8);
            r.y += cvRound(r.height*0.06);
            r.height = cvRound(r.height*0.9);

            stringstream ss_count;
            ss_count << detection_count;
            Mat cpy = input_img.clone();
            if (stateFlags.debug) imwrite("detection\ record/detection_" + ss_count.str() + ".png", cpy(Rect(r.tl(), r.br())));

            detection_count++;
            rectangle(input_img, r.tl(), r.br(), cv::Scalar(0,255,0), 2);
        }

        // Print number of bike detections
        stringstream ss;
        ss << bikes_detected;
        string number_of_bikes = ss.str();
        if (stateFlags.debug) {
		putText(input_img, number_of_bikes, Point(10,20), CV_FONT_HERSHEY_SIMPLEX, 0.5, Scalar(0, 0, 255));
        imshow("Detection Window", input_img);
        if (waitKey(20) >= 0)
            break;
        
        // Handle Alert Sign Controls
        gpioSetValue(57, on); // bandhan added this cause the program would crash with "device resource busy error"
        prevDetectionState = currentDetectionState;
        if(!found_filtered.empty()) {
            currentDetectionState = true;
            if(prevDetectionState == false && currentDetectionState == true) {
                //gpioSetValue(57, off);
                system("(sudo ./flashLEDs&) > /dev/null");
                // and update the database
                system("(sudo /home/ubuntu/Desktop/bandhan/SQL-Scripts/./BikeDetected-SQL&) > /dev/null");
                // Upload image
                system("(sudo python /home/ubuntu/Desktop/bandhan/CameraScripts/uploadS3.py&) > /dev/null");
            }
            // Save img to upload
            imwrite("latest\ detection/latest_detection.png", input_img);
        } else {
            currentDetectionState = false;
            if(prevDetectionState == true && currentDetectionState == false) {
            }
        }
    }
    
    cap.release();
}

void CallBackFunc(int event, int x, int y, int flags, void* userdata)
{
    if (event == EVENT_MOUSEMOVE) {
        mouse_x = x;
        mouse_y = y;
        cout << "( " << x << ", " << y << ")\n";
    }
}

void cameraTest () {
	namedWindow("Video Feed", WINDOW_AUTOSIZE);
	setMouseCallback("Video Feed", CallBackFunc, NULL);
	VideoCapture cap(stateFlags.cameraId);
    Mat img;
    while (waitKey(20) != 27) {
            cap.read(img);
            imshow("Video Feed", img);
    }
    imwrite("snapshot.png", img);
    destroyAllWindows();
    cap.release();
}
