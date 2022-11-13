using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class FPSPlayerController : MonoBehaviour
{
    public float m_Yaw;
    public float m_Pitch;
    public float m_PitchRotationalSpeed;
    public float m_YawRotationalSpeed;
    public float m_AirTime;
    public float m_MinPitch;
    public float m_MaxPitch;
    public float m_TotalPoints;

    public static FPSPlayerController instance;
    
    public Transform m_PitchCotroller;
    public bool m_useYawInverted;
    public bool m_UsePitchInverted;

    public CharacterController m_characterController;
    public float m_PlayerSpeed;
    public float m_FastSpeedMultiplier;
    public KeyCode m_LeftKeyCode;
    public KeyCode m_RightKeyCode;
    public KeyCode m_UpKeyCode;
    public KeyCode m_DownKeyCode;
    public KeyCode m_JumpKeyCode = KeyCode.Space;
    public KeyCode m_RunKeyCode = KeyCode.LeftShift;
    public KeyCode m_ThrowObject = KeyCode.G;
    public KeyCode m_PickObject = KeyCode.E;
    public KeyCode m_DebugLockAngleKeyCode = KeyCode.I;
    public KeyCode m_DebugLockKeyCode = KeyCode.O;


    public Camera m_Camera;
    public Camera m_CameraWeapon;
    public float m_NormalMovementFOV=60.0f;
    public float m_RunMovementFOV=75.0f;
    //public GameObject PrefabBulletHole;
    public bool m_Shooting;
    public bool m_IsReloading;
    public bool m_IsRunning;

    public Animation m_Animation;
    public AnimationClip m_IdleClip;
    public AnimationClip m_ShotClip;
    public AnimationClip m_FailShotClip;

    public bool m_PlayerIsMoving = false;
    //public TcObjectPool1 poolDecals;
    float m_VerticalSpeed = 0.0f;
    public bool m_OnGround = true; //REMOVE PUBLIC AFTER FIXED
    public float m_JumpSpeed = 10.0f;
    public bool m_AngleLocked = false;
    public bool m_AimLocked = true;
    public bool m_TargetHit = false;
    public int DecalsElements = 15;
    public Transform m_AttachingPosition;
    Rigidbody m_ObjectAttached;
    bool m_AttachingObject = false;
    Quaternion m_AttachingObjectStartRotation;
    public float m_AttachingObjectSpeed = 20f;
    public float m_MaxDistanceAttachObject = 10.0f;
    public LayerMask m_AttachObjectMask;
    public float m_ThrowAttachedObjectForce = 100f;
    public Vector3 m_Direction = Vector3.zero;
    //public Vector3 min = new(0.75f, 0.5f, 1);
    //public Vector3 max = new(1.25f, 1.5f, 1);
    [Header("Crosshairs")]
    public RawImage CrosshairEmpty;
    public RawImage CrosshairBlue;
    public RawImage CrossHairOrange;
    public RawImage CrossHairFull;
    [Header("Portals")]
    public Portal m_BluePortal;
    public Portal m_OrangePortal;
    public GameObject OrangeTexturee;
    public GameObject BlueTexturee;
    public float m_OffsetPortal = 0.5f;
    bool m_CanPassPortal;
    [Range(0.0f , 90.0f)] public float m_AngleTransformPortal; 
        
    void Start()
    {
        m_Yaw = transform.rotation.y;
        m_Pitch = m_PitchCotroller.localRotation.x;
        Cursor.lockState = CursorLockMode.Locked;
        m_AimLocked = Cursor.lockState == CursorLockMode.Locked;
        SetIdleWeaponAnimation();
        m_Shooting = false;
        m_IsReloading = false;
        m_IsRunning = false;
        instance = this;
        OrangeTexturee.SetActive(false);
        BlueTexturee.SetActive(false);
        m_BluePortal.gameObject.SetActive(false);
        m_OrangePortal.gameObject.SetActive(false);
        CrosshairBlue.gameObject.SetActive(false);
        CrossHairOrange.gameObject.SetActive(false);
        CrossHairFull.gameObject.SetActive(false);
        m_CanPassPortal = false;
        
    }

#if UNITY_EDITOR
    void UpadteInputDebug()
    {
        if (Input.GetKeyDown(m_DebugLockAngleKeyCode))
            m_AngleLocked = !m_AngleLocked;
        if (Input.GetKeyDown(m_DebugLockKeyCode))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
            m_AimLocked = Cursor.lockState == CursorLockMode.Locked;
        }
    }
#endif
    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        UpadteInputDebug();
#endif
        //Movement
        Vector3 l_RightDirection = transform.right;
        Vector3 l_ForwardDirection = transform.forward;
        m_Direction = Vector3.zero;
        float l_Speed = m_PlayerSpeed;
        float l_FOV = m_NormalMovementFOV;
        float FOV_Speed = 0.3f;
        
        if (Input.GetKey(m_UpKeyCode))
            m_Direction = l_ForwardDirection;
        if (Input.GetKey(m_DownKeyCode))
            m_Direction = -l_ForwardDirection;
        if (Input.GetKey(m_RightKeyCode))
            m_Direction += l_RightDirection;
        if (Input.GetKey(m_LeftKeyCode))
            m_Direction -= l_RightDirection;
        //Jump if SpaceBar is pressed down and player is on ground
        if (Input.GetKeyDown(m_JumpKeyCode) && m_AirTime < 0.1f)
            m_VerticalSpeed = m_JumpSpeed; 
        //Run if shift is pressed
        if (Input.GetKey(m_RunKeyCode) && m_Direction != Vector3.zero & !m_IsReloading)
        {
            //l_Speed = m_PlayerSpeed * m_FastSpeedMultiplier;
            l_FOV = m_RunMovementFOV;
            //SetRunWeaponAnimation();
            m_IsRunning = true;
            //l_FOV = m_RunMovementFOV;
        }
        if (Input.GetKeyUp(m_RunKeyCode))
        {
            //SetIdleWeaponWithRunAnimation();
            m_IsRunning = false;
        }

        if(m_Direction != Vector3.zero)
        {
            m_PlayerIsMoving = true;
        }
        else
        {
            m_PlayerIsMoving = false;
        }

        

        
        m_Camera.fieldOfView = Mathf.Lerp(m_Camera.fieldOfView, l_FOV, FOV_Speed);
        m_Direction.Normalize();

        Vector3 l_Movement = m_Direction * l_Speed * Time.deltaTime;
        
        //Rotation
        float l_MouseX = Input.GetAxis("Mouse X");
        float l_MouseY = Input.GetAxis("Mouse Y");
#if UNITY_EDITOR
        if (m_AngleLocked)
        {
            l_MouseX = 0.0f;
            l_MouseY = 0.0f;
        }
#endif
        m_Yaw = m_Yaw + l_MouseX * m_YawRotationalSpeed * Time.deltaTime*(m_useYawInverted ? -1.0f : 1.0f);
        m_Pitch = m_Pitch + l_MouseY * m_PitchRotationalSpeed * Time.deltaTime * (m_UsePitchInverted ? -1.0f : 1.0f);
        m_Pitch = Mathf.Clamp(m_Pitch, m_MinPitch, m_MaxPitch);

        transform.rotation = Quaternion.Euler(0.0f, m_Yaw, 0.0f);
        m_PitchCotroller.localRotation = Quaternion.Euler(m_Pitch, 0.0f, 0.0f);

        
        

        m_VerticalSpeed = m_VerticalSpeed + Physics.gravity.y * Time.deltaTime;
        l_Movement.y = m_VerticalSpeed * Time.deltaTime;

        CollisionFlags l_CollisionFlags = m_characterController.Move(l_Movement);
        if ((l_CollisionFlags & CollisionFlags.Above) != 0 && m_VerticalSpeed > 0.0f)
            m_VerticalSpeed = 0.0f;
        if ((l_CollisionFlags & CollisionFlags.Below) != 0) 
        {
            m_VerticalSpeed = 0.0f;
            m_OnGround = true;
            m_AirTime = 0;
        }
        else 
        {
            m_AirTime += Time.deltaTime;
            m_OnGround = false;
        }

        

        if(Input.GetKeyDown(m_PickObject) && CanAttachObject())
        {
            AttachObject();  
        }

        CrosshairPortals();

        if (m_AttachingObject)
        {
            UpdateAttachObject();
        }

        if (m_ObjectAttached && !m_AttachingObject)
        {
            if (Input.GetKeyDown(m_ThrowObject))
            {
                ThrowAttachedObject(m_ThrowAttachedObjectForce);
            }
            if (Input.GetKeyDown(m_PickObject) && !CanAttachObject())
            {
                ThrowAttachedObject(0.0f);
            }

        }
        else if(!m_AttachingObject)
        {
            if (Input.GetMouseButtonUp(0) && CanShhot())
            {
                Shoot(m_BluePortal);
                BlueTexturee.SetActive(false);
                m_BluePortal.transform.localScale = BlueTexturee.transform.localScale;
            }

            if (Input.GetMouseButtonUp(1) && CanShhot())
            {
                Shoot(m_OrangePortal);
                OrangeTexturee.SetActive(false);
                m_OrangePortal.transform.localScale = OrangeTexturee.transform.localScale;
            }

            if (Input.GetMouseButton(0))
            {
                CheckBlue();
            }

            if (Input.GetMouseButton(1))
            {
                CheckOrange();
            }

            if(Input.GetMouseButtonUp(0) && Input.GetMouseButtonUp(1))
            {
                Shoot(m_BluePortal);
            }
        }

        ScrollWheel();

        if(m_BluePortal.gameObject.activeInHierarchy && m_OrangePortal.gameObject.activeInHierarchy)
        {
            m_CanPassPortal = true;
        }
        else
        {
            m_CanPassPortal = false;
        }
    }

    bool CanAttachObject()
    {
        return m_ObjectAttached == null;
    }

    void AttachObject()
    {
        Ray l_Ray = m_Camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit l_raycastHit;
        if(Physics.Raycast(l_Ray, out l_raycastHit, m_MaxDistanceAttachObject, m_AttachObjectMask.value))
        {
            if (l_raycastHit.collider.tag =="CompanionCube")
            {
                m_AttachingObject = true;
                m_ObjectAttached = l_raycastHit.collider.GetComponent<Rigidbody>();
                m_ObjectAttached.GetComponent<Companion>().SetAttached(true);
                m_ObjectAttached.isKinematic = true;
                m_AttachingObjectStartRotation = l_raycastHit.collider.transform.rotation;
                AudioController.instance.PlayOneShot(AudioController.instance.pickupObject);
                AudioController.instance.Play(AudioController.instance.pickupObjectLoop);
            }

            if (l_raycastHit.collider.tag == "Turret")
            {
                m_AttachingObject = true;
                m_ObjectAttached = l_raycastHit.collider.GetComponent<Rigidbody>();
                m_ObjectAttached.GetComponent<Turret>().SetAttached(true);
                m_ObjectAttached.isKinematic = true;
                m_AttachingObjectStartRotation = l_raycastHit.collider.transform.rotation;
                AudioController.instance.PlayOneShot(AudioController.instance.pickupObject);
                AudioController.instance.Play(AudioController.instance.pickupObjectLoop);
            }


        }
    }
    void ThrowAttachedObject(float force)
    {
        if(m_ObjectAttached != null && m_ObjectAttached.tag == "Turret")
        {
            m_ObjectAttached.transform.SetParent(null);
            m_ObjectAttached.isKinematic = false;
            m_ObjectAttached.AddForce(m_PitchCotroller.forward * force);
            m_ObjectAttached.GetComponent<Turret>().SetAttached(false);
            m_ObjectAttached = null;
            AudioController.instance.PlayOneShot(AudioController.instance.dropObject);
            AudioController.instance.Stop(AudioController.instance.pickupObjectLoop);
        }

        if (m_ObjectAttached != null && m_ObjectAttached.tag == "CompanionCube")
        {
            m_ObjectAttached.transform.SetParent(null);
            m_ObjectAttached.isKinematic = false;
            m_ObjectAttached.AddForce(m_PitchCotroller.forward * force);
            m_ObjectAttached.GetComponent<Companion>().SetAttached(false);
            m_ObjectAttached = null;
            AudioController.instance.PlayOneShot(AudioController.instance.dropObject);
            AudioController.instance.Stop(AudioController.instance.pickupObjectLoop);
        }

    }
    void UpdateAttachObject() 
    {
        Vector3 l_EulerAngles = m_AttachingPosition.rotation.eulerAngles;
        Vector3 l_Direction = m_AttachingPosition.transform.position - m_ObjectAttached.transform.position;
        float l_Distance = l_Direction.magnitude;
        float l_Movement = m_AttachingObjectSpeed * Time.deltaTime;

        if (l_Movement >= l_Distance)
        {
            m_AttachingObject = false;
            m_ObjectAttached.transform.SetParent(m_AttachingPosition);
            m_ObjectAttached.transform.localPosition = Vector3.zero;
            m_ObjectAttached.transform.localRotation = Quaternion.identity;
            m_ObjectAttached.MovePosition(m_AttachingPosition.position);
            m_ObjectAttached.MoveRotation(Quaternion.Euler(0.0f, l_EulerAngles.y, l_EulerAngles.z));
        }
        else
        {
            l_Direction /= l_Distance;
            m_ObjectAttached.MovePosition(m_ObjectAttached.transform.position + l_Direction * l_Movement);
            m_ObjectAttached.MoveRotation(Quaternion.Lerp(m_AttachingObjectStartRotation, Quaternion.Euler(0.0f, l_EulerAngles.y, l_EulerAngles.z), 1.0f - Mathf.Min(l_Distance / 1.5f, 1.0f)));
        }


    }

    public IEnumerator EndShoot()
    {
        yield return new WaitForSeconds(m_ShotClip.length);
        m_Shooting = false;
    }
    bool CanShhot()
    {
        return !m_Shooting && !m_IsReloading;
    }
    public float m_MaxShootDistance = 50.0f;
    public LayerMask m_ShootingLayerMask;
    void Shoot(Portal _Portal)
    {
        Vector3 l_Position;
        Vector3 l_Normal;
        if(_Portal.IsValidPosition(m_Camera.transform.position, m_Camera.transform.forward,m_MaxShootDistance,m_ShootingLayerMask,out l_Position,out l_Normal))
        {
            _Portal.gameObject.SetActive(true);
            if (_Portal == m_BluePortal) AudioController.instance.PlayOneShot(AudioController.instance.weaponShootBlue);
            else if (_Portal == m_OrangePortal) AudioController.instance.PlayOneShot(AudioController.instance.weaponShootOrange);
            SetShootWeaponAnimation();
            m_Shooting = true;
            StartCoroutine(EndShoot());
        }
        else
        {
            _Portal.gameObject.SetActive(false);
            AudioController.instance.PlayOneShot(AudioController.instance.portalInvalidSurface);
            SetShootFailWeaponAnimation();
        }              
    }

    void CreatShootHitParticle(Collider collider,Vector3 position,Vector3 Normal)
    {
        if (collider.CompareTag("DronCollider"))
        {
           /// l_Decal.SetActive(false);
        }
        if (collider.CompareTag("Doors"))
        {
            //l_Decal.SetActive(false);
        }

        if (collider.CompareTag("TargetCollider"))
        {
            //l_Decal.SetActive(false);
        }

    }

    void SetIdleWeaponAnimation()
    {
        m_Animation.CrossFade(m_IdleClip.name);
    }

    void SetShootWeaponAnimation()
    {
        m_Animation.CrossFade(m_ShotClip.name,0.05f);
        m_Animation.CrossFadeQueued(m_IdleClip.name,0.05f);
    }
    void SetShootFailWeaponAnimation()
    {
        m_Animation.CrossFade(m_FailShotClip.name, 0.05f);
        m_Animation.CrossFadeQueued(m_IdleClip.name, 0.05f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeadZone"))
        {
            PlayerLife.instance.currentLife = 0;
        }

        if (other.CompareTag("Portal") && m_CanPassPortal)
        {
            Portal l_Portal = other.GetComponent<Portal>();
            if(Vector3.Dot(l_Portal.transform.forward, -m_Direction) > Mathf.Cos(m_AngleTransformPortal * Mathf.Deg2Rad))
            {
              Teleport(l_Portal);
            }
            Debug.Log("IN");
        }
    }

    private void CrosshairPortals()
    {
        if (m_BluePortal.gameObject.activeInHierarchy && m_OrangePortal.gameObject.activeInHierarchy)
        {
            CrossHairFull.gameObject.SetActive(true);
            CrosshairBlue.gameObject.SetActive(false);
            CrossHairOrange.gameObject.SetActive(false);
            CrosshairEmpty.gameObject.SetActive(false);
        }

        if (m_BluePortal.gameObject.activeInHierarchy && !m_OrangePortal.gameObject.activeInHierarchy)
        {
            CrosshairBlue.gameObject.SetActive(true);
            CrosshairEmpty.gameObject.SetActive(false);
            CrossHairOrange.gameObject.SetActive(false);
            CrossHairFull.gameObject.SetActive(false);
        }

        if (!m_BluePortal.gameObject.activeInHierarchy && !m_OrangePortal.gameObject.activeInHierarchy)
        {
            CrosshairEmpty.gameObject.SetActive(true);
            CrossHairFull.gameObject.SetActive(false);
            CrosshairBlue.gameObject.SetActive(false);
            CrossHairOrange.gameObject.SetActive(false);
        }

        if (!m_BluePortal.gameObject.activeInHierarchy && m_OrangePortal.gameObject.activeInHierarchy)
        {
            CrossHairOrange.gameObject.SetActive(true);
            CrosshairEmpty.gameObject.SetActive(false);
            CrosshairBlue.gameObject.SetActive(false);
            CrossHairFull.gameObject.SetActive(false);
        }
    }

    private void ScrollWheel()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && OrangeTexturee.transform.localScale.x < 1.25f && OrangeTexturee.activeInHierarchy)
        {
            OrangeTexturee.transform.localScale += new Vector3(0.25f, 0.25f, 0);
            
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && OrangeTexturee.transform.localScale.x > 0.75f && OrangeTexturee.activeInHierarchy)
        {
            OrangeTexturee.transform.localScale -= new Vector3(0.25f, 0.25f, 0);
            
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f && BlueTexturee.transform.localScale.x < 1.25f && BlueTexturee.activeInHierarchy)
        {
            BlueTexturee.transform.localScale += new Vector3(0.25f, 0.25f, 0);

        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f &&  BlueTexturee.transform.localScale.x > 0.75f && BlueTexturee.activeInHierarchy)
        {
            BlueTexturee.transform.localScale -= new Vector3(0.25f, 0.25f, 0);

        }

    }

    public void Teleport(Portal _Portal)
    {
        Vector3 l_Position = _Portal.m_OtherPortalTransform.InverseTransformPoint(transform.position);
        Vector3 l_Direction = _Portal.m_OtherPortalTransform.InverseTransformDirection(transform.forward);
        Vector3 l_LocalDirectionMovement = _Portal.m_OtherPortalTransform.InverseTransformDirection(m_Direction);
        Vector3 l_WorldDirectionMovement = _Portal.m_MirrorPortal.transform.TransformDirection(l_LocalDirectionMovement);
        m_characterController.enabled = false;
        transform.forward = _Portal.m_MirrorPortal.transform.TransformDirection(l_Direction);
        m_Yaw = transform.rotation.eulerAngles.y;
        transform.position = _Portal.m_MirrorPortal.transform.TransformPoint(l_Position) + l_WorldDirectionMovement * m_OffsetPortal;
        m_characterController.enabled = true;
        RandomizePortalEnterSound();
    }

    public bool OrangeTexture(Vector3 StartPosition, Vector3 forward, float MaxDistance, LayerMask PortalLayerMask, out Vector3 Position, out Vector3 Normal)
    {
        Ray l_Ray = new Ray(StartPosition, forward);
        RaycastHit l_RaycastHit;
        bool l_Valid = false;
        Position = Vector3.zero;
        Normal = Vector3.forward;

        if(Physics.Raycast(l_Ray, out l_RaycastHit, MaxDistance, PortalLayerMask.value))
        {
            if(l_RaycastHit.collider.tag == "DrawableWall")
            {
               
                l_Valid = true;
                Normal = l_RaycastHit.normal;
                Position = l_RaycastHit.point;
                OrangeTexturee.transform.position = Position;
                OrangeTexturee.transform.rotation = Quaternion.LookRotation(Normal);
                BlueTexturee.transform.position = Position;
                BlueTexturee.transform.rotation = Quaternion.LookRotation(Normal);
            }
        }

        return l_Valid;
    }

    void CheckOrange()
    {
        Vector3 l_Position;
        Vector3 l_Normal;
        if(OrangeTexture(m_Camera.transform.position, m_Camera.transform.forward, m_MaxShootDistance, m_ShootingLayerMask, out l_Position, out l_Normal))
        {
            OrangeTexturee.SetActive(true);
        }
        else
        {
            OrangeTexturee.SetActive(false);
        }
    }

    void CheckBlue()
    {
        Vector3 l_Position;
        Vector3 l_Normal;
        if (OrangeTexture(m_Camera.transform.position, m_Camera.transform.forward, m_MaxShootDistance, m_ShootingLayerMask, out l_Position, out l_Normal))
        {
            BlueTexturee.SetActive(true);
        }
        else
        {
            BlueTexturee.SetActive(false);
        }
    }
    void RandomizePortalEnterSound()
    {
        AudioController.instance.PlayOneShot(AudioController.instance.portalEnter[Random.Range(0, AudioController.instance.portalEnter.Length)]);
    }
}
