for (var number = 1; number <= 100; number++)
{
	var output = number switch
	{
		_ when number % 18 == 0 => "biométrica",
		_ when number % 6 == 0 => "bio",
		_ when number % 9 == 0 => "metrika",
		_ => number.ToString()
	};

	Console.WriteLine(output);
}
