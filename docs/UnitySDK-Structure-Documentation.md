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
2. Mystic Unity SDK package. Click [here](https://github.com/mysticswap/mystic-unity-sdk/releases) to download.
3. MetaMask Wallet Address

### Installation
1. Create new Unity Project (Any template) or open your existing project.
2. Once project is opened, export the downloaded Mystic Unity package by selecting the menu **Assets -> Import Package -> Custom Package...**<br>
![Select Custom Package](assets/images/Tutorial_Installation_1.png "Select Custom Package")
3. New window will pop-up, locate the **MysticUnitySDK.unitypackage** file.
![Select MysticUnitySD.unitypackage](assets/images/Tutorial_Installation_2.png "Select MysticUnitySD.unitypackage")
4. Import **All** items<br>
![Import All Items](assets/images/Tutorial_Installation_3.png "Import All Items")
5. Click **Import**
> At this point, Unity compiler will show the error, an external package named *newtonsoft.json* need to be installed. <br>
> ***newtonsoft.json*** is an utility package for processing json text into another data type and vice versa.

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
To get started, **MysticSDKManager** and **MetaMaskUnity** prefabs are needed to put inside the Hierarchy.<br>
* **MysticSDKManager** prefabs will grant access of the SDK functionality in your code like API call from [Mystic](https://docs.withmystic.xyz/swaps-and-marketplace-api) and it contains the wallet configuration data like Wallet Address, Authentication Token and ChainId.
* **MetaMaskUnity** prefabs is for wallet related functionality like sending transaction request to your MetaMask Mobile Apps from Unity.
* These prefabs are work as a Singleton that can be called anywhere inside your game.

To insert these prefabs, inside the scene add (drag and drop) prefabs named *MysticSDKManager* and *MetaMaskUnity* into the **Hierarchy**.

> These prefabs are located inside *Project folders* in **Assets -> Prefabs**

![Locate the prefabs](assets/images/Tutorial_SettingUp_1.png "Locate the prefabs")<br>
![Add to Hierarchy](assets/images/Tutorial_SettingUp_2.png "Add to Hierarchy")


#### Setting up your Wallet's configuration
In order of the Player to access the wallet in-game, Wallet's detail need to be adjusted. Here is how to setting up the Player's Wallet Address, and chainId. Including the AuthenticationToken for the API Call requirement.
* You could add your address inside the MysticSDKManager or you can connect it with Connect Wallet button with MetaMask Wallet Provider available on the package [here is the link to getting started]

To setting up the configuration, go to Hierarchy, click **MysticSDKManager** prefab, in the Script box you will find:<br>
![Setting up MysticSDKManager](assets/images/Tutorial_SettingUp_3.png "Setting up MysticSDKManager")
* Wallet Address: fill your Wallet Address that will be use in-game as the main address (MetaMask)
* Authentication Token: Add Bearer Authentication token (to get the token, follow the instruction [here](https://docs.withmystic.xyz/swaps-and-marketplace-api/authentication))
* Chain Id: Add available Chain Id. Supported Chain Id can be seen [here](https://docs.withmystic.xyz/supported-blockchains).

### Get Your Balance
Let's do the first call to get the balance from your address. (sdk.getBalance), here you will get ETH and WETH in json format, or sdk.GetBalanceETH to get the ETH balance and sdk.GetBalanceWETH to get the WETH balance and display it to your game [here is the sample]

To demonstrate this, let's make a scene with button interaction to do the API call.
1. Make sure to setting up the SDK, for both Scene and MysticSDKManager. [*Setting up the Mystic SDK*](https://github.com/mysticswap/mystic-unity-sdk/blob/temp/docs/docs/UnitySDK-Structure-Documentation.md#setting-up).
2. Create an **Empty GameObject**, let's name it as **ScriptManager**.<br>
![Create Empty GameObject](assets/images/Tutorial_HelloWorld_1.png "Create Empty GameObject")<br>
![Name it as ScriptManager](assets/images/Tutorial_HelloWorld_2.png "Name it as ScriptManager")
3. Put **ScriptManager.cs** script inside the GameManager.<br>
![Create a new script](assets/images/Tutorial_HelloWorld_3.png "Create a new script")
![Click Create and Add](assets/images/Tutorial_HelloWorld_4.png "Click Create and Add")
4. Create a Canvas with 3 buttons and 1 Text Mesh Pro:<br>
![Create Canvas](assets/images/Tutorial_HelloWorld_5.png "Create Canvas")<br>
![Add Buttons and Text inside Canvas](assets/images/Tutorial_HelloWorld_6.png "Add Buttons and Text inside Canvas")
    a. Button_ShowBalance: json string of balance (both ETH and WETH).<br>
    b. Button_ShowBalanceEth: balance of ETH.<br>
    c. Button_ShowBalanceWeth: balance of WETH.<br>
    d. Text_DisplayBalance: to display the output of each buttons.
5. Go to the **ScriptManager.cs** and let's do some codes:<br>
    a. Define the SDK Variable
    ```cs
    private MysticSDK _sdk;
    ```

    b. Define the GameObject Variable
    ```cs
    [SerializeField] private TextMeshProUGUI textDisplayBalance;
    ```

    c. Instantiate the SDK on `Awake()`
    ```cs
    private void Awake()
    {
        _sdk = MysticSDKManager.Instance.sdk;
    }
    ```

    d. Create buttons' method to show the balance
    ```cs
    public async void ShowBalance()
    {
        var result = await _sdk.GetBalance();
        textDisplayBalance.text = result;
    }

    public async void ShowBalanceEth()
    {
        var result = await _sdk.GetBalanceEth();
        textDisplayBalance.text = result;
    }

    public async void ShowBalanceWeth()
    {
        var result = await _sdk.GetBalanceWeth();
        textDisplayBalance.text = result;
    }
    ```
   
    e. This is the full code looks like. This code is located in `Assets/Samples/GetYourBalanceSample/Scripts/ScriptManager.cs`.
    ```cs
   using Core;
   using TMPro;
   using UnityEngine;

   public class ScriptManager : MonoBehaviour
   {
        private MysticSDK _sdk;
        [SerializeField] private TextMeshProUGUI textDisplayBalance;

        private void Awake()
        {
            _sdk = MysticSDKManager.Instance.sdk;
        }

        public async void ShowBalance()
        {
            var result = await _sdk.GetBalance();
            textDisplayBalance.text = result;
        }

        public async void ShowBalanceEth()
        {
            var result = await _sdk.GetBalanceEth();
            textDisplayBalance.text = result;
        }

        public async void ShowBalanceWeth()
        {
            var result = await _sdk.GetBalanceWeth();
            textDisplayBalance.text = result;
        }
    }
    ```
6. Apply GameObject into the **ScriptManager**.<br>
![Add Text_DisplayBalance into ScriptManager](assets/images/Tutorial_HelloWorld_7.png "Add Text_DisplayBalance into ScriptManager")
7. Apply the `ShowBalance()`, `ShowBalanceEth()` and `ShowBalanceWeth()` methods into each of the buttons.<br>
![Add OnClick() Interaction](assets/images/Tutorial_HelloWorld_8.png "Add OnClick() Interaction")<br>
![Insert ScriptManager inside OnClick()](assets/images/Tutorial_HelloWorld_9.png "Insert ScriptManager inside OnClick()")<br>
![Add Function of ShowBalance()](assets/images/Tutorial_HelloWorld_10.png "Add Function -> ScriptManager -> ShowBalance()")<br>
![Function ShowBalance added](assets/images/Tutorial_HelloWorld_11.png "ShowBalance() function will be shown")<br>
![Add ShowBalanceEth()](assets/images/Tutorial_HelloWorld_12.png "Add ShowBalanceEth() on Button_ShowBalanceEth")<br>
![Add ShowBalanceWeth()](assets/images/Tutorial_HelloWorld_13.png "Add ShowBalanceWeth() on Button_ShowBalanceWeth")<br>
8. Play the game.
9. Click the button, then you will see your balance on Text display.<br>

![Show Balance Button](assets/images/Tutorial_HelloWorld_14.png "Show All Balance")<br>
![Show Balance ETH Button](assets/images/Tutorial_HelloWorld_15.png "Show ETH Balance")<br>
![Show Balance WETH Button](assets/images/Tutorial_HelloWorld_16.png "Show WETH Balance")<br>
> CONGRATULATIONS!!! You just interacted with Web3 In-game!

### Build your first NFT swapping experience
* In this tutorial you will able to:
    * Accessing you NFTs collection inventory
    * Open swap to public or private swap (given the Requester's address)
    * Create Swap in-game
    * See all created swaps
    * Accept or cancel swap in-game
    * Make swaps work on different chains
    * Live Swaps (coming soon)

#### Using Mystic SDK Swap Experience Sample
You can find a scene sample for experiencing NFT swapping.
* Go to the Project window, **Assets → Scenes → SwapInGameSample**<br>
  ![Locate SwapInGameSample scene](assets/images/Tutorial_SwapInGameSample_1.png "Open the SwapInGameSample scene")
* On this scene there are some panels to interact with the ***items (NFTs collection or Token)***:<br>
  ![Swap Panels](assets/images/Tutorial_SwapInGameSample_2.png "Panels for swap interaction")<br>
  ![Swap Panels in-game](assets/images/Tutorial_SwapInGameSample_3.png "Swap Panel In-game looks")
  1. SDKPanel_Tabs: contains buttons to switch between the panels
     * *Create Swap* Button: to activate the SDKPanel_CreateSwap
     * *My Swap* Button: to activate the SDKPanel_MySwaps
     * *All Swap* Button: to activate the SDKPanel_AllSwaps
     * Panel_ConnectAndShowBalance: basic Wallet interactions to show balance in a Console window (`Debug.Log`) and connect your Wallet with MetaMask.
       * *Show Balance* Button: show your ETH and WETH balance in the Console window.
       * *Connect Wallet* Button: connect Wallet with MetaMask provider.
  2. SDKPanel_CreateSwap: contains offer and request panel to create swap between items.
     * Offer Panel: main player side to select their items to be trade.
     * Request Panel: other player side inventory (swap target) which main player select the items to request them.<br>
     *Notes*: there two kind of swaps which are public and private, as mentioned [here](https://docs.withmystic.xyz/swaps-and-marketplace-api/create-swap-offer),
     you can make a *private* swap by adding the address on Request side,
     otherwise leave the Request's address blank to make it *public*.
  3. SDKPanel_MySwaps: retrieve swaps owned by connected address.<br>
        On This panel, created swaps panel will be shown just like in https://mysticswap.io/sdk (in *My Swaps* section) to get the experience of 
        accept or cancel swaps in-game, you can also see Accepted or Cancelled swaps.<br>
     ![My Swap Example](assets/images/Tutorial_SwapInGameSample_4.png "Example of My Swaps looks")<br>
     * On Every single swaps contains Creator side and Taker side with button Accept/Cancel or status Accepted/Cancelled:
       * Creator Address (left side)
       * Creator Items (*NFTs collections or Token*)
       * Taker Address (right side)
       * Taker Items 
  4. SDKPanel_AllSwaps: similar as SDKPanel_MySwaps but it also display all swaps created in Mystic SDK.
     ![My Swap Example](assets/images/Tutorial_SwapInGameSample_5.png "Example of All Swaps looks")
##### Let's experience create swap in-game!
1. First, setup your [wallet configuration](https://github.com/mysticswap/mystic-unity-sdk/blob/temp/docs/docs/UnitySDK-Structure-Documentation.md#setting-up-your-wallets-configuration) inside the MysticSDKManager on Hierarchy, then **PLAY** the game. Fill the Authentication Token and Chain Id is mandatory, but for Wallet Address you can also add it by connecting to MetaMask via the ***Connect Wallet*** button 
Once the wallet is connected, NFTs collection will be shown up on Offer side.<br>
   ![NFTs collection shown](assets/images/Tutorial_ExperienceSwap_1.png "Retrieving NFTs inventory")
2. To do a private swap, add target wallet Address inside the ***Request's input field***, press enter and NFTs collection in the Request side will be shown. Meanwhile, to do a public swap, just leave the Wallet Address blank. <br>
   ![Requester NFTs inventory](assets/images/Tutorial_ExperienceSwap_2.png "Add Requester address")
3. Add items you want to swap by clicking on it, and you could add Token by pressing the ***Add Token*** button, enter the amount and click ***Confirm*** button.
    Items and Token will be added on trade box automatically to simulate in-game transaction between players.
   ![Add items to swap](assets/images/Tutorial_ExperienceSwap_3.png "Adding items to swap")<br>
   ![Add Token to swap](assets/images/Tutorial_ExperienceSwap_3a.png "Adding Token to swap")<br>
   ![Input Token to swap](assets/images/Tutorial_ExperienceSwap_4.png "Input Token to swap")<br>
   ![Show Token to trade box](assets/images/Tutorial_ExperienceSwap_4a.png "Token shown on trade box")
4. Once you are done with adding items on both side, click ***Create Swap*** button, and QR Code will be shown to connect to the MetaMask Wallet (if you didn't Connect Wallet at the beginning). Scan the QR code with your MetaMask Mobile App.<br>
   ![Create Swap Button](assets/images/Tutorial_ExperienceSwap_5.png "Click Create Swap button")<br>
   ![Connect Wallet QR scan](assets/images/Tutorial_ExperienceSwap_6.png "Scan QR Code with MetaMask App")
5. Sign the request on your MetaMask mobile apps.
6. Inside the game wait until a message created swap appears.<br>
   ![Created Swap Pop-up message](assets/images/Tutorial_ExperienceSwap_6a.png "Swap is created")
7. Click the ***My Swap*** button, and you will see the swaps you just created.<br>
   ![My Swap button](assets/images/Tutorial_ExperienceSwap_7.png "Click My Swap button")<br>
   ![Swap Created](assets/images/Tutorial_ExperienceSwap_8.png "Swap has been created")<br>
> CONGRATULATIONS!! You have just created NFTs swap in-game!

***

## Examples
    - Example Assets
    - Example Prefabs
    - Example Scripts
    - Samples

## Samples
We provide working samples for implementing features on Mystic Unity SDK.
### 1. *Get Your Balance* Sample
* Scripts: `ScriptManager.cs` contains script to do basic implementation such as calling the Mystic Unity SDK, get balance both ETH and Weth from your wallet.
* Scenes: `GetYourBalanceSample.unity` is a scene that demonstrate how to Get Balance from Mystic API with buttons to interact and display it to a Text Display.

### 2. *Swap In Game* Sample
* Game Events
  * `OnWalletOfferConnected.asset` triggered once the main player's wallet is connected to the game, and immediately show inventory of NFTs collection and token balance.
  * `OnWalletRequestConnected.asset` same as the `OnWalletOfferConnected.asset` but for the other player's side.
  * `TradeBoxRefreshItems_Offer.asset` triggered every time main player clicks the NFTs item or add Token to display the selected items or Token to the Trade Box.
  * `TradeBoxRefreshItems_Request.asset` same as the `TradeBoxRefreshItems_Offer.asset` but for other player's side.
* Prefabs
  * `Button_ConnectWallet.prefab` is a button to connect to Wallet via MetaMask provider.
  * `NFTsItem.prefab` used in Panel_Swaps as a single NFT or Token as a display on Swap Transaction.
  * `OwnedNFTButton.prefab` is a clickable NFTs item inside the player's inventory.
  * `Panel_Swaps.prefab` is a Panel to display swaps created, containing NFTsItem, swap button (Accept/Cancel) or swap status (Accepted/Cancelled) and the address.
  * `Panel_TradeBox.prefab` is a trade box to display selected NFTs item and or Token to trade (swap).
  * `TradeItem.prefab` is a single NFT item inside of Panel_TradeBox. 
* Scenes: `SwapInGameSample.unity` is a scene to demonstrate create swap between player and display created swaps.
* Scripts
  * `ConnectedWalletButton.cs`: connect to wallet via MetaMask provider, will trigger GameEvent `OnWalletOfferConnected` once connected.
  * `GameManager.cs`: main script of this scene goes here like auto connect wallet if MysticSDKManager already configured, load NFTs items, create swap, retrieve swap and more.
  * `GetRemoteTexture.cs`: class to load images of NFTs items from its ImageUrl.
  * `NFTsItem.cs`: single item inside the displayed NFT item on Trade Box.
  * `NftItem.cs`: class to load and store the data of the item.
  * `NftItems.cs`: list data of NFTs item on Offer and Request side.
  * `OwnedNFTButton.cs`: button script for the clickable NFTs item inside the player's inventory.
  * `PanelTradeBox.cs`: functionality of Trade Box (add, remove, display NFTs and token).
  * `SwapsPanel.cs`: panel script to load created swap from Mystic API.
  * `TradeItem.cs`: a single item on Trade Box (load text title and imageUrl).
  * `ITradeBox.cs`: interface for Trade Box.

***

<a name='assembly'></a>
## Scripts

## Contents

- [MysticSDK](#T-Core-MysticSDK 'Core.MysticSDK')
    - [AcceptSwap(request)](#M-Core-MysticSDK-AcceptSwap 'Core.MysticSDK.AcceptSwap')
    - [CancelSwap(request)](#M-Core-MysticSDK-CancelSwap 'Core.MysticSDK.CancelSwap')
    - [CreateSwap(request)](#M-Core-MysticSDK-CreateSwap 'Core.MysticSDK.CreateSwap')
    - [GetAddress()](#M-Core-MysticSDK-GetAddress 'Core.MysticSDK.GetAddress')
    - [GetBalance()](#M-Core-MysticSDK-GetBalance 'Core.MysticSDK.GetBalance')
    - [GetBalanceEth()](#M-Core-MysticSDK-GetBalanceEth 'Core.MysticSDK.GetBalanceEth')
    - [GetBalanceWeth()](#M-Core-MysticSDK-GetBalanceWeth 'Core.MysticSDK.GetBalanceWeth')
    - [GetMetadata(request)](#M-Core-MysticSDK-GetMetadata 'Core.MysticSDK.GetMetadata')
    - [GetNfts(_address)](#M-Core-MysticSDK-GetNfts-System-String- 'Core.MysticSDK.GetNfts(System.String)')
    - [GetOwnedNFTs(_address)](#M-Core-MysticSDK-GetOwnedNFTs-System-String- 'Core.MysticSDK.GetOwnedNFTs(System.String)')
    - [RetrieveAllSwaps(page,limit)](#M-Core-MysticSDK-RetrieveAllSwaps-System-Int32,System-Int32- 'Core.MysticSDK.RetrieveAllSwaps(System.Int32,System.Int32)')
    - [RetrieveMySwaps(page,limit,creatorAddress,takerAddress)](#M-Core-MysticSDK-RetrieveMySwaps-System-Int32,System-Int32,System-String,System-String- 'Core.MysticSDK.RetrieveMySwaps(System.Int32,System.Int32,System.String,System.String)')
    - [RetrieveSwap(swapId)](#M-Core-MysticSDK-RetrieveSwap-System-String- 'Core.MysticSDK.RetrieveSwap(System.String)')
    - [SetAddress(address)](#M-Core-MysticSDK-SetAddress-System-String- 'Core.MysticSDK.SetAddress(System.String)')

<a name='T-Core-MysticSDK'></a>
## MysticSDK `type`

##### Namespace

Core

<a name='M-Core-MysticSDK-AcceptSwap'></a>
### AcceptSwap(request) `method`

##### Summary

Accept a listed swap / offer.

##### Returns

A result of request.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| request | [M:Core.MysticSDK.AcceptSwap](#T-M-Core-MysticSDK-AcceptSwap 'M:Core.MysticSDK.AcceptSwap') | SwapData type that contains swapId and takerAddress. |

<a name='M-Core-MysticSDK-CancelSwap'></a>
### CancelSwap(request) `method`

##### Summary

Cancel a listed swap / offer.

##### Returns

A result of request.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| request | [M:Core.MysticSDK.CancelSwap](#T-M-Core-MysticSDK-CancelSwap 'M:Core.MysticSDK.CancelSwap') | SwapData type that contains swapId and takerAddress. |

<a name='M-Core-MysticSDK-CreateSwap'></a>
### CreateSwap(request) `method`

##### Summary

To create a new swap or offer on a listed NFT.

##### Returns

A result report of request.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| request | [M:Core.MysticSDK.CreateSwap](#T-M-Core-MysticSDK-CreateSwap 'M:Core.MysticSDK.CreateSwap') | CreateSwap type to be requested (please see GameManager.cs CreateSwap() to see the implementation. |

<a name='M-Core-MysticSDK-GetAddress'></a>
### GetAddress() `method`

##### Summary

Get the wallet address from the SDK session.

##### Returns

The string of address.

##### Parameters

This method has no parameters.

<a name='M-Core-MysticSDK-GetBalance'></a>
### GetBalance() `method`

##### Summary

Get the Eth and Weth Balance from the SDK session address.

##### Returns

Return a string json contains Eth and Weth in String.

##### Parameters

This method has no parameters.

<a name='M-Core-MysticSDK-GetBalanceEth'></a>
### GetBalanceEth() `method`

##### Summary

Get Eth balance from the SDK session address.

##### Returns

Eth number in string.

##### Parameters

This method has no parameters.

<a name='M-Core-MysticSDK-GetBalanceWeth'></a>
### GetBalanceWeth() `method`

##### Summary

Get Weth balance from the SDK session address.

##### Returns

Weth number in string.

##### Parameters

This method has no parameters.

<a name='M-Core-MysticSDK-GetMetadata'></a>
### GetMetadata(request) `method`

##### Summary

Retrieve detailed Metadata on a single NFTItem.

##### Returns

Detailed Metadata in json string.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| request | [M:Core.MysticSDK.GetMetadata](#T-M-Core-MysticSDK-GetMetadata 'M:Core.MysticSDK.GetMetadata') | Metadata type of the NFTItem. |

<a name='M-Core-MysticSDK-GetNfts-System-String-'></a>
### GetNfts(_address) `method`

##### Summary

Get NFTs collection from particular address.

##### Returns

String of json contains NFTs collection.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| _address | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | By default it will be the address on SDK session. |

<a name='M-Core-MysticSDK-GetOwnedNFTs-System-String-'></a>
### GetOwnedNFTs(_address) `method`

##### Summary

Get NFTs in form of OwnedNFT types (see Types.cs).

##### Returns

List of NFTs items.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| _address | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | By default it will be the address on SDK session. |

<a name='M-Core-MysticSDK-RetrieveAllSwaps-System-Int32,System-Int32-'></a>
### RetrieveAllSwaps(page,limit) `method`

##### Summary

Retrieve all existing swaps.

##### Returns

A list of swaps according to the made request.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| page | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Number of pages we want to retrieve. Default is 1. |
| limit | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Number of swaps we want to retrieve. Default is 20, limit is 100 |

<a name='M-Core-MysticSDK-RetrieveMySwaps-System-Int32,System-Int32,System-String,System-String-'></a>
### RetrieveMySwaps(page,limit,creatorAddress,takerAddress) `method`

##### Summary

Retrieve all existing swaps but only for the particular address.

##### Returns

A list of swaps according to the made request.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| page | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Number of pages we want to retrieve. Default is 1. |
| limit | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Number of swaps we want to retrieve. Default is 20, limit is 100 |
| creatorAddress | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | By default the address from SDK session. |
| takerAddress | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | By default the address from SDK session. |

<a name='M-Core-MysticSDK-RetrieveSwap-System-String-'></a>
### RetrieveSwap(swapId) `method`

##### Summary

Retrieve a single swap object.

##### Returns

The respective swap object.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| swapId | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | String of swapID. |

<a name='M-Core-MysticSDK-SetAddress-System-String-'></a>
### SetAddress(address) `method`

##### Summary

Set the address of the SDK session.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| address | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | String of address that will replace to. |



***

## Editor Tools
*Coming Soon*

***

## Glossary
*Coming Soon*