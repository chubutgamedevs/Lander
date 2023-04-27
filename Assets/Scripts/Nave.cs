using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Nave : MonoBehaviour
{
    public GameObject GM;
    Vector3 velocidad = Vector3.zero;
    public float G = 9.8f;

    public float propulsor = 20.0f;
    public float velocidadRot = 5.0f;
    [SerializeField] TextMeshProUGUI TextoVelocidad, TextoCombustible, TextoAltura;
    private float combustible;
    private float altura;
    [SerializeField] GameObject plataforma;
    [SerializeField] GameObject camara;
    private Rigidbody rb;
    private ParticleSystem trail;

    private float consumo = 0.01f;
    private bool grounded = false;
    public int limI, limD;


    void Start()
    {
        trail = GetComponent<ParticleSystem>();
        rb = GetComponent<Rigidbody>();
        StartCoroutine(HUD());
        StartCoroutine(Combustible());
        trail.Stop(true);
        Comportamiento();
        combustible = Random.Range(40,80);


    }

    void Update()
    {
        //camara.transform.LookAt(transform, Vector3.zero);

        if (!grounded)
        {
            RigidbodyController();
        }

        if (gameObject.transform.position.y < plataforma.transform.position.y)
        {
            GM.GetComponent<gamemanager>().mensaje = "Quedaste varado en la superficie lunar";
            GM.GetComponent<gamemanager>().final();

            grounded = true;
        }
        if (transform.position.x < limI)
        {
            this.transform.position = new Vector3(limD - 1, gameObject.transform.position.y, gameObject.transform.position.z);
        }
        if (transform.position.x > limD)
        {
            this.transform.position = new Vector3(limI + 1, gameObject.transform.position.y, gameObject.transform.position.z);
        }
        if (combustible <= 0){
            grounded = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        grounded = true;
        if (other.CompareTag("Finish"))
        {
            rb.constraints = RigidbodyConstraints.FreezePosition;
            

            if (rb.velocity.y < -2.0f)
            {
                GM.GetComponent<gamemanager>().mensaje = "Golpeaste muy fuerte la plataforma, mision fracasada";
            }
            else { GM.GetComponent<gamemanager>().mensaje = "Eres un heroe de esta nacion"; }

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
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.AddRelativeForce(new Vector3(-1, 0, 0) * propulsor);
            consumir();
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.AddRelativeForce(new Vector3(1, 0, 0) * propulsor);
            consumir();
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddTorque(new Vector3(0, 0, 1) * 0.5f);
            consumir();
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddTorque(new Vector3(0, 0, -1) * 0.5f);
            consumir();
        }
        trail.Play(false);
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
            trail.Play(true);
            combustible -= consumo;
        }
    }
    private void Comportamiento()
    {
        transform.position = new Vector3(Random.Range(-30, 30), Random.Range(-10, 10), 0);
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Random.Range(-40, 40)));
        rb.AddForce(new Vector3(Random.Range(-2500, 2500), 0, 0));
    }

}
