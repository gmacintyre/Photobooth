Nikon Type0004 Module SDK Revision.1 概要


■用途
 カメラのコントロールを行う。


■サポートするカメラ
 D7000


■動作環境・制限事項
 [Windows]
    Windows XP Home Edition (SP3) / Professional (SP3)
    Windows Vista SP2 32bit版/64bit版の各エディション
    (※64bit 版OS では、32bit アプリケーションとして動作)
    (Home Basic / Home Premium / Business / Enterprise / Ultimate)
    Windows 7 32bit版/64bit版の各エディション
    (※64bit 版OS では、32bit アプリケーションとして動作)
    (Home Basic / Home Premium / Professional / Enterprise / Ultimate)
    ※カメラ本体の通信モード(セットアップメニュー/USB)をPTPに設定して下さい。
      カメラがMass Storageに設定されていると、Windowsからコントロールするこ
      とはできません。

 [Macintosh]
    MacOS X 10.4.11
    MacOS X 10.5.8
    MacOS X 10.6.4
    ※カメラ本体の通信モード(セットアップメニュー/USB)をPTPに設定して下さい。
      カメラがMass Storageに設定されていると、Macintoshからコントロールするこ
      とはできません。


■内容物
 [Windows]
    Documents
      MAID3(J).pdf : 基本インターフェース仕様
      MAID3Type0004(J).pdf : Type0004 Moduleで使用される拡張インターフェース仕様
      Usage of Type0004 Module(J).pdf : Type0004 Module を使用する上での注意事項
      Type0004 Sample Guide(J).pdf : サンプルプログラムの使用方法

    Binary Files
      Type0004.md3 : Windows用 Type0004 Module本体
      NkdPTP.dll : Windows用　PTPドライバ
 
    Header Files
      Maid3.h : MAIDインターフェース基本ヘッダ
      Maid3d1.h : Type0004用MAIDインターフェース拡張ヘッダ
      NkTypes.h : 上記ヘッダで使用する型の定義

    Sample Program
      Type0004CtrlSample(Win) : Microsoft Visual Studio 2008 用プロジェクト


 [Macintosh]
    Documents
      MAID3(J).pdf : 基本インターフェース仕様
      MAID3Type0004(J).pdf : Type0004 Moduleで使用される拡張インターフェース仕様
      Usage of Type0004 Module(J).pdf : Type0004 Module を使用する上での注意事項
      Type0004 Sample Guide(J).pdf : サンプルプログラムの使用方法

    Binary Files
        Type0004 Module.bundle : Macintosh用 Type0004 Module本体 
                                                            (Universal Binary)
        libNkPTPDriver.dylib : Macintosh用 PTP ドライバ (Universal Binary)
 
    Header Files
      Maid3.h : MAIDインターフェース基本ヘッダ
      Maid3d1.h : Type0004用MAIDインターフェース拡張ヘッダ
      NkTypes.h : 上記ヘッダで使用する型の定義

    Sample Program
      Type0004CtrlSample(Mac) : Xcode 3.1.4用のサンプルプログラムプロジェクト
                                (Universal Binary)


■制限事項
 本Module SDKを利用してコントロールできるカメラは1台のみです。
 複数台のコントロールには対応していません。
