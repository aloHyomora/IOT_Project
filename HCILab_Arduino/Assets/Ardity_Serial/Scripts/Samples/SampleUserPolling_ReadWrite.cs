/**
 * Ardity (Serial Communication for Arduino + Unity)
 * Author: Daniel Wilches <dwilches@gmail.com>
 *
 * This work is released under the Creative Commons Attributions license.
 * https://creativecommons.org/licenses/by/2.0/
 */

using UnityEngine;
using System.Collections;

/**
 * Sample for reading using polling by yourself, and writing too.
 */
public class SampleUserPolling_ReadWrite : MonoBehaviour
{
    public SerialController serialController;

    // Initialization
    void Start()
    {
        serialController = GameObject.Find("SerialController").GetComponent<SerialController>();

        Debug.Log("Press A or Z to execute some actions");
    }

    // Executed each frame
    void Update()
    {
        //---------------------------------------------------------------------
        // Send data
        //---------------------------------------------------------------------

        // If you press one of these keys send it to the serial device. A
        // sample serial device that accepts this input is given in the README.
        if (AstronautPlayer.AstronautPlayer.Instance.verticalPressed)
        {
            if (Input.GetAxis("Vertical") > 0)
            {
                Debug.Log("Sending +Y");
                serialController.SendSerialMessage("1");    
            }
            else
            {
                Debug.Log("Sending -Y");
                serialController.SendSerialMessage("2");
            }
        }
        if (AstronautPlayer.AstronautPlayer.Instance.horizontalPressed)
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                Debug.Log("Sending +X");
                serialController.SendSerialMessage("3");    
            }
            else
            {
                Debug.Log("Sending -X");
                serialController.SendSerialMessage("4");
            }
        }


        //---------------------------------------------------------------------
        // Receive data
        //---------------------------------------------------------------------

        string message = serialController.ReadSerialMessage();

        if (message == null)
            return;

        // Check if the message is plain data or a connect/disconnect event.
        if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_CONNECTED) )
            Debug.Log("Connection established");
        else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_DISCONNECTED))
            Debug.Log("Connection attempt failed or disconnection detected");
        else
            Debug.Log("Message arrived: " + message);
    }
}
