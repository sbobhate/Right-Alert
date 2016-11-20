# Software Report:

## Installing OpenCV:

**OpenCV** was an essential part of our project and so we will first cover how to set it up on your machine. 

**Windows (and Visual Studio 2013):**

1.	Download the latest version of OpenCV (we used 3.0.0) and extract it to “C:/”.
2.	Right click on My Computer and go to properties.
..a.	Click Advanced System Settings
..b.	Click Environment Variables
..c.	Click New and set OPENCV_DIR against the variable name and C:\opencv\build\ against the variable name.
..d.	Click Path followed by Edit
..e.	Follow the instructions on the following blog:
http://opencv-srf.blogspot.com/2013/05/installing-configuring-opencv-with-vs.html

**NOTE:** We found Visual Studio to be troublesome so we stuck to XCode.

**Mac OS (and XCode):**

1.	Follow the following video to install OpenCV:
https://www.youtube.com/watch?v=U49CVY8yOxw&feature=youtu.be
2.	Follow the following video to configure OpenCV with XCode:
https://www.youtube.com/watch?v=XJeP1juuHHY
NOTE: You will need to configure every XCode project you by following step 2 if you want it to work with OpenCV.
3.	In XCode, you need to specify the folder that contains helper files.
..a.	Click on Product in the tool bar and then click Edit Scheme.
..b.	Check the Use Custom Directory check box.
..c.	Add the path to the folder you want to use.
This is where all the images/videos will be saved and accessed from.

LINUX (Ubuntu 14.04):

This was the operating system we used for the Jetson TKI. OpenCV was installed during the initial flashing on the OS. 
  **NOTE:** We this was OpenCV 2.4.8 and some functions differ from 3.0.0.

**NOTE:** To learn more about OpenCV, use the following link: 
http://opencv-srf.blogspot.com/2010/09/opencv-basics.html
  
## Running The Program:

The detection program can be run in a number of ways. The **out** file is the executable. Running **./out** in the terminal should give you a help message. There are several flags that can be used with the program.

**--debug**: Enable debug mode with provides some output statements and a visual record of the detection.
**-- train**: Uses the positive and negative training images in the train folder to train the SVM. The user is prompted to enter the number of positive and negative images.
**--performanceTest**: Uses the trained SVM to run detection on positive and negative images in the test folder. It then computes important performance related data like the confusion matrix and the true positive rate and false positive rate.
**--detection**: Starts the detection algorithm on a specified video or a camera feed.
**--video <filename>**: Specifies the video file to run the detection algorithm on.
**--camera <camera_id>**: Specifies the camera to use for running the detection.
**--cameraTest <camera_id>**: Runs a video capture  from the specified camera. Pressing ‘esc’ saves a snapshot of the last frame and quits the program. Moving the mouse displays the mouse coordinates in the terminal.
 
The **makefile** can be used to compile the files in the folder. Just run **make** in the terminal. Then run **./out** with and other flags you wish to use.

Examples:

	To Train:		          **./out --train**
	To Detect on a Video: **./out --detection --video test/test-video-3.avi**

The variables **top_left_x**, **top_left_y**, **roi_width** and **roi_height** in file **main.cpp** need changes to be changed every time the system is setup at a new location. The changes are to the numbers that determine the region of interest for the detection. To reduce the impact on the computational power we focus on a specific region of the frame. Use the **--cameraTest** flag to view the input to the camera. Moving the mouse displays the mouse coordinates in the frame to the terminal. Find the values that best suit the detection region. Compute the width and height of this region and enter the values into the **main.cpp**. Type **make** in the terminal and hit enter. That’s it, the code is ready to start detecting at the specified region of interest. Run **./out --detection --camera  0** to start detecting.
