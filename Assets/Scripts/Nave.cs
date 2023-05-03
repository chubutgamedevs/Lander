using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Nave : MonoBehaviour
{
    public GameObject GM;
    Vector3 velocidad = Vector3.zero;
    public float G = 9.8f;

    public float propulsor = 20.0f;
    public float velocidadRot = 5.0f;
    [SerializeField] TextMeshProUGUI TextoVelocidad, TextoCombustible, TextoAltura;
    private float combustible = 100;
    private float altura;
    [SerializeField] GameObject plataforma;
    [SerializeField] GameObject camara;
    private Rigidbody rb;

    private float consumo = 0.01f;
    private bool grounded = false;
    public int limI, limD;
    private bool lowfuel = false;
    private bool mensajeActivo = false;
    public GameObject propulsorPrin, propizq, propder, propH1, propH2, propAh1, propAh2;
    private AudioSource audioData;
    private bool propOn;


    void Start()
    {
        audioData = GetComponent<AudioSource>();
        audioData.Play(0);

        rb = GetComponent<Rigidbody>();
        StartCoroutine(HUD());
        StartCoroutine(Combustible());

        Comportamiento();
        combustible = Random.Range(45, 80);


    }

    void Update()
    {
        //camara.transform.LookAt(transform, Vector3.zero);
        Audio();
        OutOfFuel();
        Limites();
        if(!grounded)
        {
            RigidbodyController();
        }

        if (gameObject.transform.position.y < plataforma.transform.position.y)
        {
            grounded = true;
            ApagarProps();
            if (mensajeActivo == false)
            {
                GM.GetComponent<gamemanager>().mensaje = "Quedaste varado en la superficie lunar";
                GM.GetComponent<gamemanager>().final();
            }
            mensajeActivo = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        grounded = true;
        ApagarProps();
        if (other.CompareTag("Finish"))
        {


            if (mensajeActivo == false)
            {
                if (rb.velocity.y < -2.0f)
                {
                    GM.GetComponent<gamemanager>().mensaje = "Golpeaste muy fuerte la plataforma, mision fracasada";
                    mensajeActivo = true;
                }
                else
                {
                    GM.GetComponent<gamemanager>().mensaje = "Eres un heroe de esta nacion";
                    mensajeActivo = true;
                }
            }


            velocidad = Vector3.zero;
            G = 0;
        }
        GM.GetComponent<gamemanager>().final();
    }

    public IEnumerator HUD()
    {
        altura = gameObject.transform.position.y - plataforma.transform.position.y;

        TextoVelocidad.GetComponent<TMPro.TextMeshProUGUI>().text = "Velocidad Vertical: " + Mathf.Round(rb.velocity.y * 10000f) * 0.0001f + " m/s";
        TextoCombustible.GetComponent<TMPro.TextMeshProUGUI>().text = "Combustible: " + Mathf.Round(combustible * 100f) * 0.01f + "seg.";
        TextoAltura.GetComponent<TMPro.TextMeshProUGUI>().text = "Altura:" + Mathf.Round(altura * 100f) * 0.01f + "Metros";

        if (combustible < 20)
        {
            if ((lowfuel == false))
            {
                TextoCombustible.GetComponent<TMPro.TextMeshProUGUI>()
                .DOColor(Color.red, 0.6f)
                .SetLoops(-1, LoopType.Yoyo);
                lowfuel = true;
            }
        }


        yield return new WaitForSeconds(0.5f);
        StartCoroutine(HUD());
    }

    void rusticocontroller()
    {
        transform.position += Time.deltaTime * velocidad;
        velocidad += Vector3.down * G * Time.deltaTime;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            velocidad += transform.up * propulsor * Time.deltaTime;
            combustible -= 1;

        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.rotation *= Quaternion.Euler(new Vector3(0f, 0f, velocidadRot));
            combustible -= 0.05f;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.rotation *= Quaternion.Euler(new Vector3(0f, 0f, -velocidadRot));
            combustible -= 5;
        }
    }

    void RigidbodyController()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.AddRelativeForce(Vector3.up * propulsor);
            consumir();
            propulsorPrin.SetActive(true);
        }
        else
        {
            propulsorPrin.SetActive(false);
            propOn = false;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.AddRelativeForce(new Vector3(-1, 0, 0) * propulsor);
            consumir();
            propizq.SetActive(true);
        }
        else
        {
            propizq.SetActive(false);
            propOn = false;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.AddRelativeForce(new Vector3(1, 0, 0) * propulsor);
            consumir();
            propder.SetActive(true);
        }
        else
        {
            propder.SetActive(false);
            propOn = false;
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddTorque(new Vector3(0, 0, 1) * 0.5f);
            consumir();
            propAh1.SetActive(true);
            propAh2.SetActive(true);
        }
        else
        {
            propAh1.SetActive(false);
            propAh2.SetActive(false);
            propOn = false;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddTorque(new Vector3(0, 0, -1) * 0.5f);
            consumir();
            propH1.SetActive(true);
            propH2.SetActive(true);
        }
        else
        {
            propH1.SetActive(false);
            propH2.SetActive(false);
            propOn = false;
        }

    }

    public IEnumerator Combustible()
    {
        if (grounded == false)
        {
            combustible -= 1;
        }
        yield return new WaitForSeconds(1);
        StartCoroutine(Combustible());
    }
    private void consumir()
    {
        if (grounded == false)
        {
            combustible -= consumo;
            propOn = true;
        }
    }
    private void Comportamiento()
    {
        transform.position = new Vector3(Random.Range(-30, 30), Random.Range(-10, 10), 0);
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Random.Range(-40, 40)));
        rb.AddForce(new Vector3(Random.Range(-2500, 2500), 0, 0));
    }
    private void OutOfFuel()
    {
        if (combustible < 1)
        {
            combustible = 0;
            grounded = true;
            ApagarProps();
        }
    }
    private void Audio()
    {
        if (propOn == true)
        {
            audioData.UnPause();
        }
        else
        {
            audioData.Pause();
        }
    }
    private void ApagarProps()
    {
        propulsorPrin.SetActive(false);
        propizq.SetActive(false);
        propder.SetActive(false);
        propH1.SetActive(false);
        propH2.SetActive(false);
        propAh1.SetActive(false);
        propAh2.SetActive(false);
        propOn = false;
    }
    private void Limites(){
        if (transform.position.x < limI)
        {
            this.transform.position = new Vector3(limD - 1, gameObject.transform.position.y, gameObject.transform.position.z);
        }
        if (transform.position.x > limD)
        {
            this.transform.position = new Vector3(limI + 1, gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }
}
