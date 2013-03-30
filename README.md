Kineck
======

Use head and voice to control system

This project requires two additional dependencies: Microsoft.Kinect.Toolkit and Microsoft.Kinect.Toolkit.FaceTracking
They are available at: http://www.microsoft.com/en-us/kinectforwindows/develop/developer-downloads.aspx

AppKineck: The graphical user interface application that use Kinect to capture user's face, gestures and voice.

LibKineck: Accept information for AppKinect and responsible for interacting with Windows OS.

How to run:
To start the application, connect the Kinect to your computer using USB connector. 
Then run the AppKineck/bin/Release/AppKineck.exe

Project details:

  AppKineck:
  
    - FaceTrackingViewer.xaml, FaceTrackingViewer.xaml.cs: Directly copied from Sample project: "Face Tracking Basics" in "Kinect for Windows Developer Toolkit v1.6.0". To display user face mesh.
    
    - MainWindow.xaml, MainWindow.xaml.cs: Originall for from Sample project: "Face Tracking Bascis" and "Speech Basics" in "Kinect for Windows Developer Toolkit v1.6.0". The application window.

  LibKinect:
  
    - AngleCalculator.cs: Calculate head gestures
    
    - HeadRotation.cs: Enumeration of possible head gestures
    
    - KeyControl.cs: Simulate key press event and send it to OS
    
    - VolumeControl.cs: Send Volume control command to OS
    
    - Sender.cs: use to interact with AppKineck. Get information in AppKineck and call Our library methods.
    
