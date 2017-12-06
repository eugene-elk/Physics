#include <iostream>
#include <math.h>
#include <vector>

using namespace std;

struct info
{
	float R;
	float L;
	float C;
	float W;
	float w0;
	float Xc, Xl;
	float Q;
};

float UcU(int w, info& stats)
{
	float Xc = 1 / (w * stats.C);
	float Xl = w*stats.L;
	float Im = 1 / sqrt(stats.R*stats.R + ((Xl - Xc)*(Xl - Xc)));
	float Uc = Xc*Im;
	return Uc;
}

int main()
{
	info stats;
	float Rin, Lin, Cin, Win;
	cin >> Rin >> Lin >> Cin >> Win;
	stats.R = Rin;
	stats.L = Lin / 1000;
	stats.C = Cin / 1000000;
	stats.W = Win;
	stats.Xc = 1 / (stats.W * stats.C);
	stats.Xl = 1 / (stats.W * stats.L);
	stats.w0 = 1 / sqrt(stats.L * stats.C);
	stats.Q = sqrt(stats.L / stats.C) / stats.R;

	vector <float> values;

	float max = 0;

	for (int w = 1; w < stats.w0 * 2; w++)
	{
		values.push_back(UcU(w, stats));
		if (values[w - 1] > max)
			max = values[w - 1];
	}

	cout << values.size() << endl;
	cout << max << endl;

	return 0;
}