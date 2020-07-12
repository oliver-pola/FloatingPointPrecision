using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MoveScript : MonoBehaviour
{
    public GameObject[] moveItems;
    public Transform character;
    public Text textCoords;
    public Text textInfo;
    public Animator animator;
    public GameObject particles;

    public float acceleration; // m/s^2
    public float jerk; // m/s^3

    private float distance = 0.0f;
    private float speed = 1.0f;
    private Vector3 position;

    private readonly List<float> infoDistances = new List<float>();
    private readonly List<string> infoMessages = new List<string>();
    private int infoNext = 0;

    private bool active = true;
    private bool returned = false;

    // Start is called before the first frame update
    void Start()
    {
        PopulateInfo();

        //Cursor.lockState = CursorLockMode.Locked;
        particles.SetActive(active);
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();

        if (!active)
            return;

        distance += speed * Time.deltaTime;
        speed += acceleration * Time.deltaTime;
        acceleration += jerk * Time.deltaTime;

        textCoords.text = "x = y = " + distance.ToString("e");

        foreach (var item in moveItems)
        {
            position = item.transform.position;
            position.x = distance;
            position.y = distance;
            item.transform.position = position;
        }

        if (returned)
        {
            // for whatever reason it does not work to set this only once
            character.position = Vector3.down * 0.8f;
            character.rotation = Quaternion.Euler(0f, 180f, 0f);
        }


        if (infoNext < infoDistances.Count && distance > infoDistances[infoNext])
        {
            textInfo.text = infoMessages[infoNext];
            infoNext++;
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            active = true;
            particles.SetActive(active);
        }
        else if (Input.GetKeyDown("b") && !returned)
        {
            distance = 0;
            speed = 0;
            acceleration = 0;
            jerk = 0;

            particles.SetActive(false);
            //character.LookAt(Camera.main.transform.position - Vector3.up * 0.8f);
            //Camera.main.transform.LookAt(character.position + Vector3.up * 0.8f);
            animator.SetTrigger("returned");
            returned = true;

            StartCoroutine(Epilogue());
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void AddInfo(float distance, string message)
    {
        infoDistances.Add(distance);
        infoMessages.Add(message);
    }

    private void PopulateInfo()
    {
        AddInfo(1000f, "Over 1km now. Ok, actually travelled over 1.41km already, but let's not be pedantic.");
        AddInfo(10e3f, "Leaving 10km mark behind. The speed is amazing. You should try that!");
        AddInfo(60e3f, "It starts to feel a bit shaky.");
        AddInfo(120e3f, "Left earth's atmosphere. I should have taken my helmet with me.");
        AddInfo(3e5f, "Did I time-travel? I feel like an 80ies pixel character now.");
        AddInfo(1e6f, "That's not pixels, that's mosaic!");
        AddInfo(4.21e6f, "I'm just a handful of polygons now. This does not feel good.");
        AddInfo(8.4e6f, "Am I still here? Can you see me?");
        AddInfo(1e7f, "I wanted to check some geostationary satellites at 3.58e7 m. Will we make it there?");
        AddInfo(1.1e7f, "No chance getting to the moon at 3.84e8 m.");
        AddInfo(1.7e7f, "Now I totally disappeared. Please help me!");
        AddInfo(1.8e7f, "Scotty, beam me back to earth!");
        AddInfo(1.9e7f, "Your name isn't Scotty? Could you please press B to beam me home, anyway?");
        AddInfo(35786e3f, "Now we have passed the geostationary satellites.");
        AddInfo(3844e5f, "Just passed the moon.");
        AddInfo(56e9f, "Passed the orbit of Mars.");
    }

    private IEnumerator Epilogue()
    {
        textInfo.text = "Thank you! I feel and look normal again.";
        yield return new WaitForSeconds(5);
        textInfo.text = "What? This is because my model vertices are stored in local coordinates?";
        yield return new WaitForSeconds(8);
        textInfo.text = "Otherwise they would now remain totally scrambled?";
        yield return new WaitForSeconds(8);
        textInfo.text = "Whatever your technical mumbo jumbo means, It's good to be back!";
        yield return new WaitForSeconds(8);
        textInfo.text = "But one question, how could I fly to the moon and survive?";
        yield return new WaitForSeconds(8);
        textInfo.text = "What? You say I must not move? The camera and me have to remain near origin?";
        yield return new WaitForSeconds(8);
        textInfo.text = "And the moon must move to us? Yeah, come on, are you kidding me?";
        yield return new WaitForSeconds(8);
        textInfo.text = "Ok then, you do program that in. And I'll wait right here for our next trip.";
        yield return new WaitForSeconds(8);
    }
}
