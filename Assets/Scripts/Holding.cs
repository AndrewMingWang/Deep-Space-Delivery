using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Holding : Building
{
    // How much does a player entering the hitbox change the scale?
    float _ScaleDelta;
    float _MaxYScale;
    Dog _primeDog = null;
    bool _finishedHolding = false;
    List<GameObject> _seen = new List<GameObject>();

    [Header("Threshold")]
    public int ThresholdDogCount = 4;
    public int DogCount = 0;
    public int MaxThreshold = 9;
    public float MinYScale;

    [Header("Appearance")]
    public TMP_Text ThresholdText;
    public List<GameObject> Pillars;
    public ParticleSystem Particles;

    private void Start()
    {
        _MaxYScale = Pillars[0].transform.localScale.y;
        _ScaleDelta = (_MaxYScale - MinYScale) / ThresholdDogCount;
        ThresholdText.text = "0/" + ThresholdDogCount.ToString();

        foreach (GameObject pillar in Pillars)
        {
            pillar.GetComponent<MeshRenderer>().material.SetColor("_Color", SelectedColor);
        }
    }

    public void IncrementThreshold()
    {
        if (ThresholdDogCount == MaxThreshold)
        {
            return;
        }
        ThresholdDogCount += 1;
        ThresholdText.text = "0/" + ThresholdDogCount.ToString();
        _ScaleDelta = (_MaxYScale - MinYScale) / ThresholdDogCount;
    }

    public void DecrementThreshold()
    {
        if (ThresholdDogCount == 1)
        {
            return;
        }
        ThresholdDogCount -= 1;
        ThresholdText.text = "0/" + ThresholdDogCount.ToString();
        _ScaleDelta = (_MaxYScale - MinYScale) / ThresholdDogCount;
    }

    public void TriggerEntered(Collider other)
    {
        if (!_finishedHolding)
        {
            // Debug.Log("enter");
            if (other.CompareTag("player") && !_seen.Contains(other.gameObject))
            {
                _seen.Add(other.gameObject);
                DogCount += 1;

                foreach (GameObject pillar in Pillars)
                {
                    pillar.transform.localScale -= _ScaleDelta * transform.up;
                    pillar.transform.localPosition = new Vector3(
                        pillar.transform.localPosition.x,
                        pillar.transform.localScale.y / 2,
                        pillar.transform.localPosition.z);
                }

                ThresholdText.text = DogCount.ToString() + "/" + ThresholdDogCount.ToString();

                Dog dog = other.GetComponent<Dog>();

                if (_primeDog == null)
                {
                    _primeDog = dog;
                    _primeDog.StopPlayer();
                    _primeDog.Animator.SetBool("idle", true);
                }
                else
                {
                    _primeDog.SetNumPackages(_primeDog.NumPackages + 1);

                    // TODO: Replace this with some animation
                    StartCoroutine(SetInactive(dog));
                }
            }
            if (DogCount == ThresholdDogCount)
            {
                _finishedHolding = true;
                _primeDog.Animator.SetBool("idle", false);
                _seen.Clear();
                StartCoroutine(ReleasePrime());
            }
        }
    }

    private IEnumerator ReleasePrime()
    {
        yield return new WaitForSeconds(0.5f);

        _primeDog.UnstopPlayer();
    }

    private IEnumerator SetInactive(Dog dog)
    {
        _primeDog.transform.localScale *= 1.05f;
        dog.gameObject.SetActive(false);
        Particles.transform.position = dog.transform.position + dog.transform.up * 0.15f;
        Particles.Play();
        yield return new WaitForSeconds(0f);
        /*
        Rigidbody dogRB = dog.GetComponent<Rigidbody>();
        dogRB.useGravity = false;
        dogRB.freezeRotation = false;
        dogRB.maxAngularVelocity = 300f;

        dog.StopPlayer();
        dog.SetNumPackages(0);
        
        yield return new WaitForSeconds(0.15f);
        
        dogRB.AddForce(dog.transform.up * 10, ForceMode.VelocityChange);
        if (Random.Range(0f, 1f) > 0.5f)
        {
            dogRB.AddTorque(new Vector3(0, 8, 0), ForceMode.VelocityChange);
        } else
        {
            dogRB.AddTorque(new Vector3(0, -8, 0), ForceMode.VelocityChange);
        }
        
        dog.Animator.SetTrigger("jumpaway");
        yield return new WaitForSeconds(3.0f);
        dog.gameObject.SetActive(false);
        */
    }

    public override void Awake()
    {
    }

    public override void setColorPickedUp()
    {
        foreach (GameObject pillar in Pillars)
        {
            pillar.GetComponent<MeshRenderer>().material.SetColor("_Color", SelectedColor);
        }
    }

    public override void setColorPlaced()
    {
        foreach (GameObject pillar in Pillars)
        {
            pillar.GetComponent<MeshRenderer>().material.SetColor("_Color", PlacedColor);
        }
    }

    public override void PlaceBuilding()
    {
        setColorPlaced();
    }

    public override void PickUpBuilding()
    {
        setColorPickedUp();
    }

    public override void Reset()
    {
        base.Reset();
        _finishedHolding = false;
        _primeDog = null;
        DogCount = 0;
        ThresholdText.text = "0/" + ThresholdDogCount.ToString();

        foreach (GameObject pillar in Pillars)
        {
            pillar.transform.localScale = new Vector3(
                pillar.transform.localScale.x,
                _MaxYScale,
                pillar.transform.localScale.z);
            pillar.transform.localPosition = new Vector3(
                pillar.transform.localPosition.x,
                pillar.transform.localScale.y / 2,
                pillar.transform.localPosition.z);
        }
    }
}
