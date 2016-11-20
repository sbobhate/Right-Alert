# Right-Alert
A Collection of the Documentation for my Senior Design Project

* **Shantanu Bobhate**
* **Brian Tan**
* **Bandhan Zishanuzzaman**
* **John McCullough**
* **Teng Zhang**

## Motivation

In the city of Boston, where countless bikers share the same streets as cars every day, bike safety is of utmost importance. Unfortunately, each year there are numerous collisions between bicycles and cars and the results can be fatal. One of the most dangerous situations bikers find themselves in is when cars make right turns across the bike lane. Sometimes drivers fail to check their side-view mirrors before turning, and as a result crash into bikers. Our team, RightAlert, provides a solar-powered system to warn drivers of oncoming bikers when a collision like this is imminent. A camera detects bikers traveling down the bike lane while an LED-lit traffic alert sign functions as a visual cue for drivers letting them know of the oncoming bike traffic. The RightAlert system is an intuitive tool for all drivers turning at an intersection. RightAlert provides a website which displays real time data while the system is running. It also allows for remote administration of specific system settings. Our system rivals alternative technologies that dedicate themselves to warning only one vehicle and fail to log traffic information. We at RightAlert are making Boston a haven for both bikers and drivers and seek to further improve overall traffic research in the city by providing our findings.

[![](https://img.youtube.com/vi/Dha42Zwq1EA/0.jpg)](https://www.youtube.com/watch?v=Dha42Zwq1EA)

## Technical Overview

Our system uses an Nvidia Jetson TK1 as the primary processor. It uses a wide angle camera with adjustable zoom and focus as the main method of vision. The system runs a Machine Learning algorithm that trains an SVM using HOG features of training images, and uses the resulting descriptor vector to classify bikes. The system is sustainable as it has a battery to power itself and a Solar Panel that provides sustainable recharging. The Alert Sign is a standardized traffic control sign, but with high-intensity flashing LEDs to command the driver’s attention. The system also collects data about the bike traffic and stores it in a database. The information is displayed to our website hosted on AWS, admin.rightalert.net.

## 
