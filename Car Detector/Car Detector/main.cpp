//
//  main.cpp
//  Car Detector
//
//  Created by Shantanu Bobhate on 1/22/16.
//  Copyright © 2016 Shantanu Bobhate. All rights reserved.
//

#include <iostream>
#include <opencv2/opencv.hpp>

int main(int argc, const char * argv[]) {
    
    std::cout << "Car Detector" << std::endl;
    
    cv::Mat image = cv::imread("Test Image.jpg");
    if (image.empty()) {
        std::cout << "Error: Could not read image." << std::endl;
        return -1;
    }
    cv::imshow("Car Detector", image);
    cv::waitKey(0);
    
    return 0;
}
