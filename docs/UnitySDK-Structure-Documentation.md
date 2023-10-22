# MysticSwap Unity SDK

***

## Getting Started

### About MysticSwap Unity SDK
* Is a package on Unity to help connect Game Development with Web3 such as trade NFTs collection in-game, or microtransaction with blockchain.

### How it Works
* Flow how MysticSwap Untiy SDK (from in-game interaction to API Call)
* Explain it with graph contains layer in Unity (game dev interact with), layer from Mystic SDK side
* This SDK is based on this https://docs.withmystic.xyz/

### OnBoarding
* How to use this docs, what should understand before building Web3 Game
* Basic Web3 interaction such as trading NFTs collection, but don't worry since in this guide is pretty straight forward with the project samples

***

## Tutorial(s)

### Overview
Here is what you need to prepare:
1. Unity (any version should works)
2. Mystic Unity SDK package (_link coming soon_) `[TODO: make package's downloadable link]`
3. MetaMask Wallet Address

### Installation
1. Create new Unity Project (Any template) or open your existing project.
2. Once project is opened, export the downloaded Mystic Unity package by selecting the menu **Assets -> Import Package -> Custom Package...**<br>
![Select Custom Package](assets/images/Tutorial_Installation_1.png "Select Custom Package")
3. New window will pop-up, locate the **MysticUnitySDK.unitypackage** file.
![Select MysticUnitySD.unitypackage](assets/images/Tutorial_Installation_2.png "Select MysticUnitySD.unitypackage")
4. Import **All** items
![Import All Items](assets/images/Tutorial_Installation_3.png "Import All Items")
5. Click **Import**
> At this point, Unity compiler will show the error, an external package need to be installed. 

> *coming soon to install this external package on MysticSDK Manager Tools*

To install the external package (newtonsoft.json), follow this step:

1. Select the menu **Window -> Package Manager**<br>
![Select Package Manager](assets/images/Tutorial_Installation_4.png "Select Package Manager")
2. At the *Package Manager* window, click the **+** icon at the top-left corner
![Add package from git URL...](assets/images/Tutorial_Installation_5.png "Add package from git URL...")
3. Choose **Add package from git URL...**
4. Add this inside the URL text box: `com.unity.nuget.newtonsoft-json`
![Add com.unity.nuget.newtonsoft-json as URL](assets/images/Tutorial_Installation_6.png "Add com.unity.nuget.newtonsoft-json as URL")
5. Click **Add**
6. Please wait until the installation finished
7. Check the Console window inside the Unity Editor, make sure there is no error occurred.

### Setting up

#### Setting up the Scene
To get started, inside the scene add (drag and drop) prefabs named *MysticSDKManager* and *MetaMaskUnity* into the **Hierarchy**.
![Locate the prefabs](assets/images/Tutorial_SettingUp_1.png "Locate the prefabs")
![Add to Hierarchy](assets/images/Tutorial_SettingUp_2.png "Add to Hierarchy")
> These prefabs are located inside *Project folders* in **Assets -> Prefabs**

These prefabs work as a Singleton that can be called anywhere inside your game.

#### Setting up your Wallet's configuration
* You could add your address inside the MysticSDKManager or you can connect it with Connect Wallet button with MetaMask Wallet Provider available on the package [here is the link to getting started]

    To setting up the configuration, go to Hierarchy, click **MysticSDKManager** prefab, in the Script box you will find:<br>
![Setting up MysticSDKManager](assets/images/Tutorial_SettingUp_3.png "Setting up MysticSDKManager")
* Wallet Address: fill your Wallet Address that will be use in-game as the main address (MetaMask)
* Authentication Token: Add Bearer Authentication token (can be found in here) `[TODO: add link]`
* Chain Id: Add available Chain Id `[TODO: add list of Chain Id along with its Contract Address]`

### Hello World
* Let's do the first call to get the balance from your address. (sdk.getBalance), here you will get ETH and WETH in json format, or sdk.GetBalanceETH to get the ETH balance and sdk.GetBalanceWETH to get the WETH balance and display it to your game [here is the sample]

### Build your first NFT swapping experience
* In this tutorial you will able to:
    * Accessing you NFTs collection inventory
    * Open swap to public or private swap (given the Requester's address)
    * Create Swap in-game
    * See all created swaps
    * Accept or cancel swap in-game
    * Make swaps work on different chains
    * Live Swaps (coming soon)

***

## Examples
    - Example Assets
    - Example Prefabs
    - Example Scripts
    - Samples

***

## Scripts: to include Mysticswap lists of API Calls and how to use it on Unity
    - For each scripts will contains:
        - Short Description
        - Example usage
        - Parameters description
        - Return Value description

***

## Editor Tools
    - MysticSwap SDK Manager
    - ...

***

## Glossary