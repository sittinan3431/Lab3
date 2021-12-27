using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 1.0f;
    public int selectedWeaponIndex;

    //List
    //public List<WeaponBehaviour> weapons = new List<WeaponBehaviour>();

    //Array
    public WeaponBehaviour[] weapons;
    public int arraySize;


    void Start()
    {
        References.thePlayer = gameObject;
        weapons = new WeaponBehaviour[1];
        for (int index = 0; index < arraySize; index++)
        {
            weapons[index] = new WeaponBehaviour();
        }
        selectedWeaponIndex = 0;
    }


    // Update is called once per frame
    void Update()
    {

        //การเคลื่อนที่ของแท่นปืน
        Vector3 inputVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Rigidbody ourRigidBody = GetComponent<Rigidbody>();
        ourRigidBody.velocity = inputVector * speed;
        //การหมุนแท่นปืนเป็นวงกลม
        Ray rayFromCameraToCursor = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        playerPlane.Raycast(rayFromCameraToCursor, out float distanceFromCamera);
        Vector3 cursorPosition = rayFromCameraToCursor.GetPoint(distanceFromCamera);
        gameObject.transform.LookAt(cursorPosition);

        //กำหนดให้กดปุ่มเม้าซ้ายยิงปืน
        if (weapons.Length > 0 && Input.GetButton("Fire1"))
        {
            //Tell our weapon to fire
            weapons[selectedWeaponIndex].Fire(cursorPosition);
        }

        //ทำการเปลี่ยนอาวุธกดปุ่มเมาขวา
        if (Input.GetButtonDown("Fire2"))
        {
            ChangeWeaponIndex(selectedWeaponIndex + 1);
        }
    }

    private void ChangeWeaponIndex(int index)
    {

        //Change our index
        selectedWeaponIndex = index;
        //If it's gone too far, loop back around
        if (selectedWeaponIndex >= weapons.Length)
        {
            selectedWeaponIndex = 0;
        }

        //For each weapon in our list
        for (
            int i = 0; //Declare a variable to keep track of how many iterations we've done
            i < weapons.Length; //Set a limit for how high this variable can go
            i++ //Run this after each time we iterate - increase the iteration count
        )
        {
            if (i == selectedWeaponIndex)
            {
                //If it's the one we just selected, make it visible - 'enable' it
                weapons[i].gameObject.SetActive(true);
            }
            else
            {
                //If it's not the one we just selected, hide it - disable it.
                weapons[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        WeaponBehaviour theirWeapon = other.GetComponentInParent<WeaponBehaviour>();
        if (theirWeapon != null)
        {
            //Add it to our internal list
            //weapons.Add(theirWeapon);

            //Add it to our internal Array
            weapons[selectedWeaponIndex] = theirWeapon;
            //Move it to our location
            theirWeapon.transform.position = transform.position;
            theirWeapon.transform.rotation = transform.rotation;
            //Parent it to us - attach it to us, so it moves with us
            theirWeapon.transform.SetParent(transform);
            //Select it!
            ChangeWeaponIndex(weapons.Length - 1);
        }
    }
}