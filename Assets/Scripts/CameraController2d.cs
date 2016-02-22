using UnityEngine;
using System.Collections;

public class CameraController2d : MonoBehaviour {
    public GameObject Player1, Player2;
    public Camera mainCam;
    public float Zoomspeed = 1.2f;
    public float maxZoom = 9.0f;
    public float minZoom = 7.0f;
    public float maxX = 3.0f;
    public float minX = -3.0f;
    public float maxY = 7.0f;
    public float minY = 3.0f;
    private Vector3 Distance;
    private Vector3 CamPosition;
    private float posx, posy, posz, camSize;
    
     
    // Use this for initialization
    void Start () {
       
    }

    // Update is called once per frame


    void LateUpdate()
    {
        // la distance entre les deux players.
        if (Player1.transform.position.y >= Player2.transform.position.y)
        {
            Distance = Player2.transform.position - Player1.transform.position;
        }
        else
        {
            Distance = Player1.transform.position - Player2.transform.position;
        }
        

        // if 2 players are alive then {
       
        //set de la variable camSize par rapport à la distance entre les deux players.
        camSize = Distance.magnitude * Zoomspeed;

        //if dépasse le max zoom
        if (camSize > maxZoom)
        {
            camSize = maxZoom;

            posx = (Player1.transform.position.x + Player2.transform.position.x) / 2;
            posy = (Player1.transform.position.y + Player2.transform.position.y) / 2;

            posz = -10.0f;

            if (posx > maxX)
                posx = maxX;
            else {
                if (posx< minX)
                    posx = minX;
            }
            if (posy > maxY)
                posy = maxY;
            else
            {
                if (posy < minY)
                    posy = minY;
            }
               

            CamPosition.Set(posx, posy, posz);
            transform.position = CamPosition;

        }
        else
            if (camSize < minZoom)
        {
            camSize = minZoom;

            posx = (Player1.transform.position.x + Player2.transform.position.x) / 2;
            posy = (Player1.transform.position.y + Player2.transform.position.y) / 2;

            posz = -10.0f;

            if (posx > maxX)
                posx = maxX;
            else {
                if (posx < minX)
                    posx = minX;
            }
            if (posy > maxY)
                posy = maxY;
            else
            {
                if (posy < minY)
                    posy = minY;
            }


            CamPosition.Set(posx, posy, posz);
            transform.position = CamPosition;
        }


        else
        {
            posx = (Player1.transform.position.x + Player2.transform.position.x) / 2;
            posy = (Player1.transform.position.y + Player2.transform.position.y) / 2;

            posz = -10.0f;

            if (posx > maxX)
                posx = maxX;
            else {
                if (posx < minX)
                    posx = minX;
            }
            if (posy > maxY)
                posy = maxY;
            else
            {
                if (posy < minY)
                    posy = minY;
            }


            CamPosition.Set(posx, posy, posz);
            transform.position = CamPosition;
        }

        mainCam.orthographicSize = camSize;
         
         


        // }
    }
}
