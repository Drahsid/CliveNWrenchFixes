using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using UnityStandardAssets.Cameras;

namespace CliveNWrenchFixes {
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin {
        private ConfigEntry<int> MaxFramerate;
        private ConfigEntry<float> CameraSpeedX;
        private ConfigEntry<float> CameraSpeedY;
        private CameraFollow Follow;


        private void Awake() {
            MaxFramerate = Config.Bind("General", "MaxFramerate", 240, "Limit the game's maximum framerate");
            CameraSpeedX = Config.Bind("General", "CameraSpeedX", 110.0f, "Speed of the camera rotation (game default is 110)");
            CameraSpeedY = Config.Bind("General", "CameraSpeedY", 30.0f, "Speed of the camera rotation (game default is 30)");
        }

        private void Update() {
            float frametime = Time.deltaTime;
            float maxframetime = 1.0f / (float)MaxFramerate.Value;
            float frametimescale = frametime / maxframetime;

            Application.targetFrameRate = MaxFramerate.Value;
            QualitySettings.vSyncCount = 0;

            if (Follow == null) {
                GameObject mainCamera = GameObject.Find("_Clive Seagull's Main Camera");

                if (mainCamera != null) {
                    Follow = mainCamera.GetComponentInChildren<CameraFollow>();
                }
            }

            // since the camera rotation is incorrectly scaled to the framerate in the game, we will counteract it here by scaling the rotation speed to the framerate
            if (Follow != null) {
                Follow.rightstickSpeed.y = CameraSpeedY.Value * frametimescale;
                Follow.rightstickSpeed.x = CameraSpeedX.Value * frametimescale;
                Follow.myRigAngleSpeed = CameraSpeedX.Value;
                Follow.rightstickStopSpeed.x = Follow.rightstickSpeed.x;
                Follow.rightstickStopSpeed.y = Follow.rightstickSpeed.y;
            }
        }
    }
}

