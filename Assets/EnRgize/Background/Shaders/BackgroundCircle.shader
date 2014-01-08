// Base shader from http://wiki.unity3d.com/index.php?title=UnlitAlphaWithFade
// Blend mode suggestions from https://github.com/prime31/UnityCgShaderTutorials

Shader "Custom/BackgroundCircle"
{
    Properties
    {
        _Color ("Color Tint", Color) = (1,1,1,1)
        _MainTex ("Base (RGB) Alpha (A)", 2D) = "white"
    }
 
    Category
    {
        // Render before standard Transparent since this is the background
        // Removing this may break sprites
        Tags { "Queue" = "Transparent-1" }
        
        Lighting Off
        ZWrite Off
        //ZWrite On // uncomment if you have problems like the sprite disappear in some rotations.
        Cull back
        
        // Blend Equation:
        //    SrcFactor * fragment_output + DstFactor * pixel_color_in_framebuffer;
        
        Blend SrcAlpha DstAlpha
        //Blend SrcAlpha OneMinusSrcAlpha // alpha blending
        
        // ---------------------------------------------------------------
        //Blend OneMinusSrcAlpha Zero
        //Blend SrcAlpha OneMinusDstAlpha
        //Blend OneMinusSrcAlpha OneMinusSrcAlpha
        
        //Blend SrcAlpha OneMinusSrcAlpha // alpha blending
        //Blend One OneMinusSrcAlpha      // premultiplied alpha blending
        //Blend One One                   // additive
        //Blend SrcAlpha One              // additive blending
        //Blend OneMinusDstColor One      // soft additive
        //Blend DstColor Zero             // multiplicative
        //Blend DstColor SrcColor         // 2x multiplicative
        //Blend Zero SrcAlpha             // multiplicative blending for attenuation by the fragment's alpha
        //Blend SrcColor OneMinusSrcAlpha
        
        //AlphaTest Greater 0.001 // uncomment if you have problems like the sprites or 3d text have white quads instead of alpha pixels.
        Tags {Queue=Transparent}
 
        SubShader
        {
            Pass
            {
                SetTexture [_MainTex]
                {
                    ConstantColor [_Color]
                    Combine Texture * constant
                }
            }
        }
    }
}
