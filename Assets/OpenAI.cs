using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class OpenAI : MonoBehaviour
{
    class RequestData
    {
        public string animation;
        public string dialogue;
    }

    string Base = "http://10.75.0.150:5000";
    string animation = "";
    string dialogue = "";

    async Task SoundAsync()
    {
        RequestData data = new RequestData();
        data.animation = animation;
        data.dialogue = dialogue;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
