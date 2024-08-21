Unity Standard Demo Scene

How to run it:
- open demo scene 
- regenerate light in lighting window
- import post process stack 2.0 into your project if you need image effects (package manager--> post process stack )
- click play
- play with directional light angle at x = 40 it looks pretty nice.
- change shadow distance to 500 or higher in quality settings
- play with wind prefab and wind speed:)
- you can change anisotropic textures to "forced on" at project settings -> quality this will make scene look much better but older device can notice fps drop

Best performance:
- Set linear color space
- Set deferred render mode. This will increate performance beacuse of reflection probes alot
- If you want to use forward rendering disable reflection probes as they are heavy at forward and open space scene. 
  Screen space reflection at post process stack should replace them in most cases
- You could disable few features at post processing stack like:
   * Depth of field
   * Screen Space Reflections which are pretty heavy 
   * Play with ambient occlusion type
   * Turn off motion blur
 - Change Anti-aliasing at camera
 - Check anisotropic textures at project settings -> quality
