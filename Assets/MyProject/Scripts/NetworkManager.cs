using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;



public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance; //singleton, um unico objeto com essa classe(NetworkManager)

    [SerializeField] GameObject playerPrefabTeamRed;
    [SerializeField] GameObject playerPrefabTeamBlue;
    [HideInInspector] public Transform cameraPlayer;

    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        ConnectToPhoton();
        Debug.Log("Start");
    }

    public void ConnectToPhoton() 
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("ConnectToPhoton");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("OnConnectedToMaster");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null);
        Debug.Log("OnJoinRandomFailed");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        Debug.Log("Playercount: " + PhotonNetwork.CurrentRoom.PlayerCount);

        photonView.RPC("CreatePlayerAvatar", PhotonNetwork.LocalPlayer);
            //photonView, chamado remoto
    }

    [PunRPC]
    //pode chamar o metodo de forma remota, chamar no computador alheio ou só de voce ou de uma pessoa especifica. Parecido com serializedfield, se põe na frente
    void CreatePlayerAvatar() 
    {
     
        Vector3 _pos = new Vector3(Random.Range(-3f, 3f), 2f, Random.Range(-3f, 3f));
            //posições aleatorias

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            //Instantiate
            //instantiate normal é local
            PhotonNetwork.Instantiate(playerPrefabTeamRed.name, _pos, Quaternion.identity);
            //quaternion controla a rotação, 4 valores
        }
        else 
        {
            PhotonNetwork.Instantiate(playerPrefabTeamBlue.name, _pos, Quaternion.identity);
        }
        
        
    }



}
