//
//  main.cpp
//
//  Created by Shantanu Bobhate on 2/18/16.
//

#include <stdio.h>
#include <opencv2/opencv.hpp>
#include "functions.hpp"
#include <iostream>

using namespace std;
using namespace cv;

/* Change these values when you first setup the system */
int top_left_x = 250, top_left_y = 330, roi_width = 50, roi_height = 110;

int main(int argc, const char * argv[]) {
    
    setup(argc, argv);
    
    if (stateFlags.train) {
        int num_pos;
        printf("Enter number of positive images: ");
        cin >> num_pos;
        printf("Extracting Features...\n");
        extractFeatures("pos", num_pos);
        printf("Done extracting positive features.\n");
        int num_neg;
        printf("Enter number of negatives images: ");
        cin >> num_neg;
        extractFeatures("neg", num_neg);
        printf("Done extracting negative features.\n");
        printf("Done with feature extraction.\n");
        printf("Training SVM...\n");
        trainSVM();
        printf("SVM has been trained.\n");
    }
    
    if (stateFlags.performanceTest) {
        int positives, negatives;
        printf("Enter number of positives: ");
        cin >> positives;
        printf("Enter number of negatives: ");
        cin >> negatives;
        printf("Performing Performance Tests...\n");
        performanceTest( positives, negatives);
        printf("Completed Performance Testing.\n");
    }
    
    if (stateFlags.detection) {
        printf("Starting Bike Detection...\n");
        printf("Beginning to export and set direction of gpio57.\n");
        gpioExport(57);
        gpioSetDirection(57, outputPin);  
        findBikes(top_left_x, top_left_y, roi_width, roi_height);
        printf("Done with Bike Detection.\n");
    }

    if (stateFlags.cameraTest) {
        printf("Starting Camera Test...\n");
        cameraTest();
        printf("Done with Camera Test.\n");
    }

    printf("Quitting Program.\n");

    return 0;
}
