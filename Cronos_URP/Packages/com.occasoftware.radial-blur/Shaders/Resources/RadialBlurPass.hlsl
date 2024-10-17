#ifndef OS_RADIAL_BLUR_PASS_INCLUDED
#define OS_RADIAL_BLUR_PASS_INCLUDED

/////////////////////////////////////////////
//   Properties                            //
/////////////////////////////////////////////
float2 _Center;
float _Intensity;
int _SampleCount;
float _Delay;

TEXTURE2D_X(_Source);
SAMPLER(sampler_Source);

/////////////////////////////////////////////
//   Methods                               //
/////////////////////////////////////////////
// Returns % between start and stop
half InverseLerp(half start, half stop, half value)
{
	return (value - start) / (stop - start);
}

half Remap(half inStart, half inStop, half outStart, half outStop, half v)
{
	half t = InverseLerp(inStart, inStop, v); 
	return lerp(outStart, outStop, saturate(t));
}

float3 RadialBlur(float2 uv)
{
	const float MAX_DISTANCE = 2.828;
	float intensity = (_Intensity * _Intensity) * 0.2;

	float2 screenPos = 1.0 - (uv * 2.0);
	float2 uvNormalizedAround0 = screenPos;
	screenPos -= _Center;
	
	float dist = distance(_Center, uvNormalizedAround0);
	
	float3 srcColor = _Source.SampleLevel(sampler_Source, uv, 0).rgb;
	float3 outputColor = srcColor;
	float inverseSampleCount = rcp((float)_SampleCount);
	float intensityPerSample = intensity * inverseSampleCount;
	float2 offsetDirection = normalize(screenPos);
	
	float offsetAmount = Remap(_Delay, MAX_DISTANCE, 0.0, 1.0, dist) * intensityPerSample;
	
	if(offsetAmount > 0)
	{
		for (int i = 1; i <= _SampleCount; i++)
		{	
			float2 offset = offsetDirection * offsetAmount * i;
			float2 targetCoord = uv + offset;
		
			outputColor += _Source.SampleLevel(sampler_Source, targetCoord, 0).rgb;
		}
	
		outputColor *= inverseSampleCount;
	}
	return outputColor;
}

           
/////////////////////////////////////////////
//   Fragment                              //
///////////////////////////////////////////// 
float3 Fragment (Varyings input) : SV_Target
{
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

    return RadialBlur(input.texcoord);
}


#endif