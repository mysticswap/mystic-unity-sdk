# Mystic Unity SDK Architecture
## Big Picture
```mermaid
flowchart TD
    Web3[Web3]
    MS[Mystic SDK]
    WP[Wallet Provider]
    MSM[Mystic SDK Manager]
    API[API Call]
    W[Wallet]
    
    Web3 --> MS
    Web3 --> WP
    MS <--> MSM
    WP <--> MSM
    
    subgraph Web3
        API
        W
    end
    
    subgraph UNITY
        MS
        WP
        subgraph User
            MSM
        end
    end
```
Mystic Unity SDK will do the api call to the Web3 with parameter given at Unity (Mystic SDK Manager), Wallet Provider separated from the Mystic SDK will do the Wallet thing from Web3 and give the data (wallet address) to the Mystic SDK Manager then store it. 

Just keep it mind that Mystic SDK and Wallet Provider will be provided on Unity as a package.

In the future, address and another parameters will be used on Mystic SDK to interact with Web3 such as Get_Balance, Get_NFTs, Swap, etc.

## Code Architecture
```mermaid
classDiagram
    
    MonoBehaviour <|-- MysticSDKManager
    MonoBehaviour <|-- MysticSDK
    MonoBehaviour <|-- Debugger
    MonoBehaviour <|-- GameEventListener
    MonoBehaviour <|-- EventManagerSample
    
    ScriptableObject <|-- StringVariable
    ScriptableObject <|-- GameEvent
    
    Editor <|-- EventEditor
    
    MysticSDKManager *-- MysticSDK
    
    MysticSDK "1" --> "1..*" Debugger
    EventManagerSample "1" --> "1..*" Debugger
    EventManagerSample "1" --> "1..*" GameEventListener

    namespace Unity {
        class MonoBehaviour
        class ScriptableObject
        class Editor
        
    }
    namespace Core {
        class MysticSDKManager{
            +sdk : MysticSDK
            +Instance : MysticSDKManager static
            -Awake()
        }
        class MysticSDK{
            -walletAddress : StringVariable
            -authenticationToken : StringVariable
            -chainId : string
            -uri: string const
            +GetBalance()
            +GetNfts()
            -CallRequest(endpointRequest: string)
            -EndpointRequest(endpoint: string, parameter: params stringp[]): string
            -GetRequest(uri: string, authenticationToken: string): IEnumerator
            -AttachHeader(webRequest: UnityWebRequest, key: string, value: string)
        }
        class Debugger{
            +debuggerPanel : GameOject
            +titleText : TMP_Text
            +descriptionText : TMP_Text
            +Instance : Debugger static
            -Awake()
            -Start()
            +Log(title: string, description: string)
        }
    }
    namespace EventManager {
        class StringVariable{
            +developerDescription : string
            +value : string
            +SetValue(value: string)
            +SetValue(value: StringVariable)
        }
        class GameEventListener{
            +Event : GameEvent
            +Response : UnityEvent
            -OnEnable()
            -OnDisable()
            +OnEventRaised()
        }
        class GameEvent{
            -eventListeners : List~GameEventListener~ readonly
            +Raise()
            +RegisterListener(listener: GameEventListener)
            +UnregisterListener(listener: GameEventListener)
        }
        class EventEditor{
            +OnInspectorGUI()
        }
        class EventManagerSample{
            +TestEvent()
        }
    }
```

The user will interact with the prefab of MysticSDKManager (singleton), instantiate it and ready to use everywhere in-game. It should provide the data such as authenticationToken, walletAddress, chainID, etc.