//
// Created by regha on 28.02.2025.
//

#ifndef FUNC_H
#define FUNC_H


class Func
{
public:
	Func(double xMinFunc, double xMaxFunc)
		: p_xMinFunc(xMinFunc), p_xMaxFunc(xMaxFunc) {}

	virtual ~Func() = default;

	void Draw()
	{
		DrawImpl();
	}

protected:
	virtual void DrawImpl() = 0;

	double p_xMinFunc;
	double p_xMaxFunc;
};


#endif //FUNC_H