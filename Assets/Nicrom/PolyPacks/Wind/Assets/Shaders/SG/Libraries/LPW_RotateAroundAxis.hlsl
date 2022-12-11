void RotateAroundAxis_float(float3 center, float3 original, float3 u, float angle, out float3 newPos)
{
	original -= center;
	float C = cos(angle);
	float S = sin(angle);
	float t = 1 - C;
	float m00 = t * u.x * u.x + C;
	float m01 = t * u.x * u.y - S * u.z;
	float m02 = t * u.x * u.z + S * u.y;
	float m10 = t * u.x * u.y + S * u.z;
	float m11 = t * u.y * u.y + C;
	float m12 = t * u.y * u.z - S * u.x;
	float m20 = t * u.x * u.z - S * u.y;
	float m21 = t * u.y * u.z + S * u.x;
	float m22 = t * u.z * u.z + C;
	float3x3 finalMatrix = float3x3(m00, m01, m02, m10, m11, m12, m20, m21, m22);
	
	newPos = mul(finalMatrix, original) + center;
}