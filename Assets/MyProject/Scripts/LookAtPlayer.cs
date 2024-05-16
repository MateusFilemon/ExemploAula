using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LookAtPlayer : MonoBehaviourPun
{

    private void Update()
    {
        RotateToPlayer();
    }


    void RotateToPlayer() 
    {
        transform.LookAt(NetworkManager.instance.cameraPlayer.position);
        //acessa a variavel unica instance, que representa a classe networkmanager, e extrai tudo dela inclusive a cameraplayer, que é publica
    }


}
