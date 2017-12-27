using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionInteractionTest2 : MonoBehaviour {

    public GameObject ActionPanel;
    public GameObject InventoryPanel;
    public GameObject RubyIcon;
    public GameObject textRaccogliRubino;
    public GameObject textInserisciRubino;

    GameObject instanceTextRaccogliRubino;
    GameObject instanceTextInserisciRubino;

    private void OnTriggerEnter(Collider other)
    {



        //istanziare il bottone
        if (other.gameObject.tag == "rubino")
        {

            instanceTextRaccogliRubino = Instantiate(textRaccogliRubino);
            instanceTextRaccogliRubino.transform.SetParent(ActionPanel.transform);


        }

        if (other.gameObject.tag == "laser")
        {
            //controllo se c'è il rubino nell'inventario

            foreach (Transform uiElement in InventoryPanel.transform)
            {
                //se c'è il rubino nell'inventario esce faccio uscire la scritta 
                if (uiElement.tag == "rubinoInventario")
                {

                    instanceTextInserisciRubino = Instantiate(textInserisciRubino);
                    instanceTextInserisciRubino.transform.SetParent(ActionPanel.transform);



                }

            }

        }
    }

    private void OnTriggerStay(Collider other)
    {

        //Raccoglie l'oggetto
        if (other.gameObject.tag == "rubino" && Input.GetKeyDown(KeyCode.E))
        {
            //"Cancello" il rubino dalla scena
            other.gameObject.transform.GetComponent<BoxCollider>().enabled = false;
            other.gameObject.transform.GetComponent<MeshRenderer>().enabled = false;

            //Inserisco il rubino nell'inventario

            GameObject i = Instantiate(RubyIcon);

            //Fare il controllo

            i.transform.SetParent(InventoryPanel.transform);



            Debug.Log("Premuto");
            //cancello il bottone

            Destroy(instanceTextRaccogliRubino.gameObject);



        }

        //Inserisci il rubino se presente nell'inventario
        if (other.gameObject.tag == "laser" && Input.GetKeyDown(KeyCode.E) && instanceTextInserisciRubino != null)
        {
            //Rimuovo il rubino dall'inventario


            foreach (Transform uiElement in InventoryPanel.transform)
            {
                //se c'è il rubino lo rimuovo
                if (uiElement.tag == "rubinoInventario")
                {
                    Debug.Log("trovato");
                    Destroy(uiElement.gameObject);
                    Debug.Log("eliminato ui element");

                }

            }






            //Attivo il laser

            GameObject laser = GameObject.Find("LaserGatto");
            laser.GetComponent<BasicInteraction>().m_hasHead = true;


            //cancello il testo

            Destroy(instanceTextInserisciRubino.gameObject);
            Debug.Log("Premuto");


        }

    }

    private void OnTriggerExit(Collider other)
    {
        foreach (Transform uiElement in ActionPanel.transform)
        {
            //distruggere il bottone una volta usciti dalla collisione, se non è stato premuto obv
            if (other.gameObject.tag == "rubino" && uiElement.tag == "textRaccogliRubino")
            {


                Destroy(uiElement.gameObject);
            }

            if (other.gameObject.tag == "laser" && uiElement.tag == "textInserisciRubino")
            {


                Destroy(uiElement.gameObject);
            }

        }
    }
}
