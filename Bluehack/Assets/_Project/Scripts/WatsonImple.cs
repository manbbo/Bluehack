using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

namespace IBM.Watson.DeveloperCloud.Widgets
{
    public class WatsonImple : MonoBehaviour
    {
        private TextToSpeechWidget textToSpeechWidget;
        private SpeechToTextWidget speechToTextWidget;
        private SpeechDisplayWidget speechDisplayWidget;
        private JsonParser jParser;

        void Start()
        {
            textToSpeechWidget = GetComponent<TextToSpeechWidget>();
            speechToTextWidget = GetComponent<SpeechToTextWidget>();
            speechDisplayWidget = GetComponent<SpeechDisplayWidget>();
            jParser = GetComponent<JsonParser>();
            speechDisplayWidget.m_Output.onValueChanged.AddListener((value) => {
                string newText = speechDisplayWidget.m_Output.text;
                print(newText);
                changeEvtState(newText);
            });
        }

        enum getWatsonState { start, location, video, info, hotels }

        public void setListenStatus(bool status)
        {
            speechToTextWidget.Active = status;
        }

        getWatsonState gWS = getWatsonState.start;
        //getWatsonMiddleState gWMS = getWatsonMiddleState.menu;

        bool onVoiceReceived(string key, string speaker)
        {
            if (speaker.Contains(key))
            {
                setListenStatus(false);
                return true;
            }

            return false;
        }

        public void onTextReceived()
        {
            textToSpeechWidget.OnTextToSpeech();
        }

        public void changeEvtState(string newText)
        {
            switch (gWS)
            {
                case getWatsonState.start:
                    foreach (string j in jParser.locations())
                    {
                        if (onVoiceReceived(j, newText) )
                        {
                            gWS = getWatsonState.location;
                            Globals.actualLocation = j;
                            print("Acessou LOCALIZACAO " + j);
                            return;
                        }
                    }
                    break;

                case getWatsonState.location:
                    if (onVoiceReceived("back", newText) )
                    {
                        gWS = getWatsonState.location;
                        Globals.actualLocation = null;
                        return;
                    }
                    else if (onVoiceReceived("video", newText) )
                    {
                        gWS = getWatsonState.location;
                        print("Acessou VIDEOOOOO " + "video");
                        Globals.actualVideo = null;
                        return;
                    }
                    else if (onVoiceReceived("info", newText) )
                    {
                        gWS = getWatsonState.location;
                        print("Acessou INFOOOOOO " + "info");
                        Globals.actualLocation = null;
                        return;
                    }
                    else{
                        List<string> temp = new List<string>();
                        foreach (string b in temp)
                        {
                            if (onVoiceReceived(b, newText))
                            {
                                gWS = getWatsonState.hotels;
                                print("Acessou HOLET " + b);
                                Globals.actualHotels = b;
                            }
                        }
                        return;
                    }
                    //Put ifs for video, hotels, info
                    break;

                case getWatsonState.video:

                    if (onVoiceReceived("back", newText) )
                    {

                        gWS = getWatsonState.location;

                        Globals.actualHotels = null;
                        return;

                    }
                    break;

                case getWatsonState.info:

                    if (onVoiceReceived("back", newText) )
                    {

                        gWS = getWatsonState.location;
                        return;

                    }

                    break;
                case getWatsonState.hotels:


                    if (onVoiceReceived("back", newText) )
                    {
                        gWS = getWatsonState.location;
                        Globals.actualHotels = null;
                        return;
                    }
                    break;
            }
            return;
        }

    }

    public static class Globals
    {
        public static string actualLocation, actualHotels, actualVideo;
    }
}