//
//  primal_svm.hpp
//  
//
//  Created by Shantanu Bobhate on 3/17/16.
//
//

#ifndef primal_svm_hpp
#define primal_svm_hpp

#include <stdio.h>
#include <vector>
#include <opencv2/opencv.hpp>

class PrimalSVM: public CvSVM {
public:
    void getSupportVector( std::vector<float> & support_vector );
};

#endif /* primal_svm_hpp */
