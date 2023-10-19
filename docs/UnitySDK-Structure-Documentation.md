# MysticSwap Unity SDK

* Getting Started
    - About MysticSwap Unity SDK
        - Is a package on Unity to help connect Game Development with Web3 such as trade NFTs collection in-game, or microtransaction with blockchain.
    - How it works: Flow how MysticSwap Untiy SDK (from in-game interaction to API Call)
        - explain it with graph contains layer in Unity (game dev interact with), layer from Mystic SDK side
    - OnBoarding: how to use this docs, what should understand before building Web3 Game
        - Basic Web3 interaction such as trading NFTs collection, but don't worry since in this guide is pretty straight forward with the project samples

* Tutorial(s)
    - Overview
        - What you need to prepare is Unity, package Mystic Unity SDK (here is the link)
    - Installation
        - Open Unity, export custom package, and we are ready to go!
    - Setting up
        - On the folder from package, you will see .../Prefabs just drag and drop the MysticSDKManager into your scene. This prefab works as a singleton that can be called anywhere in your game.
        - You could add your address inside the MysticSDKManager or you can connect it with Connect Wallet button with MetaMask Wallet Provider available on the package [here is the link to getting started]
    - Hello World
        - Let's do the first call to get the balance from your address. (sdk.getBalance), here you will get ETH and WETH in json format, or sdk.GetBalanceETH to get the ETH balance and sdk.GetBalanceWETH to get the WETH balance and display it to your game [here is the sample]
    - Build your first NFT swapping experience
        - In this tutorial you will able to:
            - Accessing you NFTs collection inventory
            - Open swap to public or private swap (given the Requester's address)
            - Create Swap in-game
            - Accept or cancel swap in-game

* Examples
    - Example Assets
    - Example Prefabs
    - Example Scripts
    - Samples

* Scripts: to include Mysticswap lists of API Calls and how to use it on Unity
    - For each scripts will contains:
        - Short Description
        - Example usage
        - Parameters description
        - Return Value description

* Editor Tools
    - MysticSwap SDK Manager
    - ...

* Glossary