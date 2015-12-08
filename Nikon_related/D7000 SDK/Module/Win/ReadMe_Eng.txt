Nikon Type0004 Module SDK Revision.1 summary


Usage
 Control the camera.


Supported camera
 D7000


Environment of operation
 [Windows]
    Windows XP Home Edition (SP3) / Professional (SP3)
    Windows Vista SP2 32bit/64bit edition
    (*Works 32 bit application on 64 bit OS)
    (Home Basic / Home Premium / Business / Enterprise / Ultimate)
    Windows 7 32bit/64bit edition
    (*Works 32 bit application on 64 bit OS)
    (Home Basic / Home Premium / Professional / Enterprise / Ultimate)
    * Please set Camera to PTP mode by SET UP menu (on camera body). If the 
      camera is set to Mass Storage, you can not control it by Windows.

 [Macintosh]
    MacOS X 10.4.11
    MacOS X 10.5.8
    MacOS X 10.6.4
    * Please set Camera to PTP mode by SET UP menu (on camera body). If the 
      camera is set to Mass Storage, you can not control it by Macintosh.


Contents
 [Windows]
    Documents
      MAID3(E).pdf : Basic interface specification
      MAID3Type0004(E).pdf : Extended interface specification used 
                                                              by Type0004 Module
      Usage of Type0004 Module(E).pdf : Notes for using Type0004 Module
      Type0004 Sample Guide(E).pdf : The usage of a sample program

    Binary Files
      Type0004.md3 : Type0004 Module for Win
      NkdPTP.dll : Driver for PTP mode used by Win

    Header Files
      Maid3.h : Basic header file of MAID interface
      Maid3d1.h : Extended header file for Type0004 Module
      NkTypes.h : Definitions of the types used in Maid3.h.

    Sample Program
      Type0004CtrlSample(Win) : Project for Microsoft Visual Studio 2008


 [Macintosh]
    Documents
      MAID3(E).pdf : Basic interface specification
      MAID3Type0004(E).pdf : Extended interface specification used by 
                                                                Type0004 Module
      Usage of Type0004 Module(E).pdf : Notes for using Type0004 Module
      Type0004 Sample Guide(E).pdf : The usage of a sample program

    Binary Files
        Type0004 Module.bundle : Type0004 Module for Mac (Universal Binary)
        libNkPTPDriver.dylib : PTP driver for Mac (Universal Binary)
 
    Header Files
      Maid3.h : Basic header file of MAID interface
      Maid3d1.h : Extended header file for Type0004 Module
      NkTypes.h : Definitions of the types used in Maid3.h.

    Sample Program
      Type0004CtrlSample(Mac) : Sample program project for Xcode 3.1.4.
                                                             (Universal Binary)


Limitations
 This module cannot control two or more cameras.
