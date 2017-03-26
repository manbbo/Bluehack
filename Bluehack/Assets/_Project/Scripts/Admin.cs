using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IBM.Watson.DeveloperCloud.Widgets
{
    public class Admin : MonoBehaviour
    {
		private WatsonImple wImplementation;

		//I put those variables bellow into the WatsonImple class. Just putting it here
        //private TextToSpeechWidget textToSpeechWidget;
        //private SpeechToTextWidget speechToTextWidget;

        // Use this for initialization
        void Start()
        {
            wImplementation = GetComponent<WatsonImple>();
            //textToSpeechWidget = GetComponent<TextToSpeechWidget>();
            //speechToTextWidget = GetComponent<SpeechToTextWidget>();
        }

        // Update is called once per frame
        //void Update()
        //{
        //    if (!wImplementation.getListeningStatus()) {
        //        wImplementation.changeEvtState();
        //        wImplementation.onTextReceived();
        //        wImplementation.setListenStatus(true);
        //    }
        //}
    }
}