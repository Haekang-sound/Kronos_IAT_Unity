README
================================

Contents
--------------------------------
About
Installation Instructions
Usage Instructions
Public API
Troubleshooting
Requirements
Support
Feedback


About
--------------------------------
Radial Blur is a Renderer Feature and API that enables you to easily use Radial (Zoom) Blur in your Unity URP project. You can customize the Intensity of the effect, the Sample Count, and the Center (Origin) point.


Installation Instructions
--------------------------------
1. Import the Radial Blur asset to your project.
2. Navigate to the active Forward Renderer Data asset in use in your project. This asset is typically named "ForwardRendererData".
3. Click "Add Renderer Feature", and select "Radial Blur Feature" from the dropdown menu.


Usage Instructions
--------------------------------
1. Create a Global Volume in your scene and add a Profile.
2. Click "Add Override", OccaSoftware -> Radial Blur
3. Configure the override
4. When you want to control the Radial Blur, call one of the public methods present in the Radial Blur Post Process component. These methods are described in more detail below. An example is provided in the Samples folder.

N.B. The Radial Blur Post Process is a member of the OccaSoftware.RadialBlur.Runtime namespace. You must include a using OccaSoftware.RadialBlur.Runtime directive in classes where you would like to interface with this component.


Public API
--------------------------------
The RadialBlurManager class includes the following public methods. These can be viewed directly in source in the .cs file.

# SetIntensity
public void SetIntensity(float intensity);
Sets the intensity of the Radial Blur filter

# GetIntensity
public float GetIntensity();
Gets the intensity of the Radial Blur filter

# SetCenter 
public void SetCenter(Vector2 center);
Sets the Center (Origin) of the Radial Blur filter. [0,0] is the Screen Center.

# SetCenterFromScreenPoint
public void SetCenterFromScreenPoint(Vector2 screenPoint);
Sets the Center (Origin) of the Radial Blur filter from screen point coordinates, measured [0,0] at bottom left to [Screen.width, Screen.height] at top right.

# GetCenter
public Vector2 GetCenter();
Gets the current Center (Origin) of the Radial Blur filter. [0,0] is the Screen Center.

# SetSampleCount
public void SetSampleCount(int sampleCount);
Sets the number of target number of samples to be used for the Radial Blur filter.

# GetSampleCount
public int GetSampleCount();
Gets the current number of samples being used for the Radial Blur filter.

# GetDelay
public float GetDelay();
Gets the current delay being used.

# SetDelay
public void SetDelay(float delay);
Sets the start offset to be used for the Radial Blur filter.


Troubleshooting
--------------------------------
1. Verify that the Radial Blur Renderer Feature is included in your Forward Renderer Data asset.
2. Verify that you have a Radial Blur override present in your scene.
3. Verify that you have a RadialBlurShader present in your project.


Requirements
--------------------------------
This asset is designed for Unity 2021.3+ Universal Render Pipeline.


Support
--------------------------------
We're dedicated to 100% customer satisfaction. 
If you need any support, please contact us at hello@occasoftware.com or join our Discord (https://www.occasoftware.com/discord).


Feedback
--------------------------------
Your feedback is extremely important to us. 
Please let us know what you think by sharing a review for this asset on the asset store (https://u3d.as/2LSk).
