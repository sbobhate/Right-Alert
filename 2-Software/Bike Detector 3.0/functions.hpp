//
//  functions.hpp
//  
//
//  Created by Shantanu Bobhate on 3/17/16.
//
//

#ifndef functions_hpp
#define functions_hpp

#include <stdio.h>
#include <iostream>
#include <vector>
#include <string>
#include <opencv2/opencv.hpp>
#include "primal_svm.hpp"
#include <opencv2/gpu/gpu.hpp>
#include "jetsonGPIO.h"

using namespace std;
using namespace cv;

/*
 * Struct to hold program arguments
 */
struct flags {
    bool debug;
    bool train;
    bool performanceTest;
    bool detection;
    int cameraId;
    string videoFile;
    bool cameraTest;
};

/*
 * Struct declaration
 */
extern flags stateFlags;

/*
 * Function to setup the flags depending on the user arguments
 */
void setup(const int argc, const char** argv);

/*
 * Function to print usage help
 */
void printHelp();

/*
 * Function to extract hog features from the positive
 *  and negative datasets
 *  - folder: the 'pos' or 'neg' folder
 *  - numberOfImages: size of dataset
 */
void extractFeatures (std::string folder, int numberOfImages);

/*
 * Function to train the SVM
 */
void trainSVM ();

/*
 * Function to measure detection performance
 *  - result: what is the ground truth for the images
 *  - numberOfTestImages: The number images to test
 */
void performanceTest (int numberOfPosTestImages, int numberOfNegTestImages);

/*
 * Function to perform detection
 * - takes the dimensions of the region of interest as it's arguments
 */
void findBikes (int top_left_x, int top_left_y, int roi_width, int roi_height);

/*
 * Helper function for cameraTest function
 */
void CallBackFunc(int event, int x, int y, int flags, void* userdata);

/*
 * Function to test camera feed
 * - takes a snapshot when you press 'esc' and quits the program
 * - moving mouse cursor outputs mouse coordinates relative to the frame
 * This function is used to setup at a new location
 */
void cameraTest ();

#endif /* functions_hpp */
