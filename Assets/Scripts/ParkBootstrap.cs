using System;
using System.Collections.Generic;
using UnityEngine;

namespace TinyThemePark
{
    /// <summary>Creates an entire small 3D theme park when Play is pressed in an empty scene.</summary>
    public sealed class ParkBootstrap : MonoBehaviour
    {
        private readonly List<Transform> rides = new List<Transform>();
        private readonly List<Transform> guests = new List<Transform>();
        private ParkEconomy economy;
        private Camera sceneCamera;
        private float simulationClock;
        private string notice = "Bem-vindo ao seu parque!";
        private readonly Vector3[] rideLocations =
        {
            new Vector3(-7f, 0f, -6f),
            new Vector3(7f, 0f, -6f),
            new Vector3(-7f, 0f, 7f),
            new Vector3(7f, 0f, 7f),
            new Vector3(0f, 0f, 0f),
            new Vector3(15f, 0f, 0f),
            new Vector3(-15f, 0f, 0f),
            new Vector3(0f, 0f, 15f),
            new Vector3(0f, 0f, -15f)
        };

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Launch()
        {
            if (FindObjectOfType<ParkBootstrap>() == null)
                new GameObject("TinyThemeParkRuntime").AddComponent<ParkBootstrap>();
        }

        private void Start()
        {
            economy = new ParkEconomy();
            SetupScene();
            CreateRide(rideLocations[0], new Color(0.99f, 0.57f, 0.26f));
            SpawnGuests();
        }

        private static GameObject Part(
            PrimitiveType shape, Transform parent, string label,
            Vector3 localPosition, Vector3 localScale, Color tint)
        {
            var obj = GameObject.CreatePrimitive(shape);
            obj.name = label;
            obj.transform.SetParent(parent, false);
            obj.transform.localPosition = localPosition;
            obj.transform.localScale = localScale;
            var renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = new Material(Shader.Find("Standard"));
                renderer.material.color = tint;
            }
            return obj;
        }

        private void SetupScene()
        {
            sceneCamera = Camera.main;
            if (sceneCamera == null)
            {
                var cam = new GameObject("ParkCamera");
                sceneCamera = cam.AddComponent<Camera>();
            }
            sceneCamera.clearFlags = CameraClearFlags.SolidColor;
            sceneCamera.backgroundColor = new Color(0.47f, 0.74f, 0.88f);
            sceneCamera.fieldOfView = 56f;

            var lightObject = new GameObject("Sunlight");
            var sunlight = lightObject.AddComponent<Light>();
            sunlight.type = LightType.Directional;
            sunlight.intensity = 1.5f;
            sunlight.transform.rotation = Quaternion.Euler(55f, -30f, 0f);

            Part(PrimitiveType.Plane, transform, "Grass", Vector3.zero, new Vector3(5f, 1f, 5f), new Color(0.21f, 0.55f, 0.30f));
            Part(PrimitiveType.Cube, transform, "Central Walkway", new Vector3(0, 0.04f, 0), new Vector3(4.5f, 0.09f, 44f), new Color(0.77f, 0.67f, 0.52f));
            Part(PrimitiveType.Cube, transform, "Cross Walkway", new Vector3(0, 0.05f, 0), new Vector3(44f, 0.1f, 4.5f), new Color(0.77f, 0.67f, 0.52f));
            for (int i = 0; i < 12; i++)
            {
                float a = (float)(i * Math.PI * 2.0 / 12.0);
                float x = Mathf.Cos(a) * 22f;
                float z = Mathf.Sin(a) * 22f;
                Part(PrimitiveType.Cylinder, transform, "GardenTreeTrunk", new Vector3(x, 1.4f, z), new Vector3(.35f, 1.4f, .35f), new Color(.38f, .23f, .12f));
                Part(PrimitiveType.Sphere, transform, "GardenTreeCrown", new Vector3(x, 3.4f, z), Vector3.one * 2.7f, new Color(.13f, .39f, .23f));
            }
        }

        private void CreateRide(Vector3 where, Color color)
        {
            var anchor = new GameObject("SpinningAttraction");
            anchor.transform.SetParent(transform, false);
            anchor.transform.localPosition = where;
            Part(PrimitiveType.Cylinder, anchor.transform, "Platform", new Vector3(0,.25f,0), new Vector3(2.7f,.25f,2.7f), new Color(.21f,.29f,.48f));
            Part(PrimitiveType.Cylinder, anchor.transform, "CentralColumn", new Vector3(0,1.7f,0), new Vector3(.3f,1.5f,.3f), color);
            Part(PrimitiveType.Cylinder, anchor.transform, "Roof", new Vector3(0,3.4f,0), new Vector3(2.3f,.27f,2.3f), color);
            for(int i=0;i<6;i++)
            {
                float a=i*Mathf.PI/3f;
                var p=new Vector3(Mathf.Cos(a)*1.8f,1.4f,Mathf.Sin(a)*1.8f);
                Part(PrimitiveType.Sphere,anchor.transform,"RideSeat",p,Vector3.one*.75f,
                    i%2==0?new Color(.93f,.91f,.38f):new Color(.43f,.76f,.95f));
            }
            rides.Add(anchor.transform);
        }

        private void SpawnGuests()
        {
            for (int i=0;i<15;i++)
            {
                var guest=Part(PrimitiveType.Capsule,transform,"Visitor",Vector3.zero,
                    new Vector3(.48f,.73f,.48f),new Color(.45f+(i%4)*.12f,.38f+(i%3)*.17f,.68f));
                guests.Add(guest.transform);
            }
        }

        private void TryAddAttraction()
        {
            if (economy.Attractions >= rideLocations.Length)
            {
                notice = "Todas as areas disponiveis estao ocupadas.";
                return;
            }
            if (!economy.TryBuildRide())
            {
                notice = "Moedas insuficientes para uma nova atracao.";
                return;
            }
            var shade = Color.HSVToRGB((economy.Attractions * .19f) % 1f, .65f, .98f);
            CreateRide(rideLocations[economy.Attractions - 1], shade);
            notice = "Nova atracao inaugurada!";
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B)) TryAddAttraction();
            simulationClock += Time.deltaTime;
            if (simulationClock >= 8f)
            {
                simulationClock -= 8f;
                economy.SimulateMinute();
            }
            for(int i=0;i<rides.Count;i++) rides[i].Rotate(Vector3.up,Time.deltaTime*(23f+i*3f),Space.Self);
            for(int i=0;i<guests.Count;i++)
            {
                float a=Time.time*(.13f+(i%4)*.018f)+i*Mathf.PI*2f/guests.Count;
                float radius=11f+(i%3)*2.6f;
                guests[i].localPosition=new Vector3(Mathf.Cos(a)*radius,.78f,Mathf.Sin(a)*radius);
                guests[i].localRotation=Quaternion.Euler(0f,-a*Mathf.Rad2Deg,0f);
            }
            float t=Time.time*.055f;
            sceneCamera.transform.position=new Vector3(Mathf.Sin(t)*30f,24f,Mathf.Cos(t)*30f);
            sceneCamera.transform.LookAt(new Vector3(0,1.2f,0));
        }

        private void OnGUI()
        {
            if (economy == null) return;
            GUI.Box(new Rect(16,16,342,216),"TINY THEME PARK — MVP");
            GUI.Label(new Rect(34,49,310,25),"Moedas: " + economy.Coins);
            GUI.Label(new Rect(34,76,310,25),"Visitantes: " + economy.Visitors);
            GUI.Label(new Rect(34,103,310,25),"Atracoes: " + economy.Attractions);
            GUI.Label(new Rect(34,130,310,25),"Satisfacao: " + economy.Happiness + "%");
            GUI.Label(new Rect(34,158,310,25),"Nova atracao: " + economy.BuildCost + " moedas");
            if (GUI.Button(new Rect(34,184,300,32),"CONSTRUIR ATRACAO (B)")) TryAddAttraction();
            GUI.Box(new Rect(16,244,342,48),notice);
        }
    }
}
