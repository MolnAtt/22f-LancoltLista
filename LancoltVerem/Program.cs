using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LancoltVerem
{
	internal class Program
	{

		class LancoltLista
		{

		}

		class Elem
		{
			Elem bal;
			int tartalom;
			Elem jobb;

			public Elem(Elem bal, int tartalom, Elem jobb)
			{
				this.bal = bal;
				this.tartalom = tartalom;
				this.jobb = jobb;
			}

			public Elem() // ő itt a fejelem, ha nem adunk meg semmit
			{
				this.bal = this;
				this.tartalom = 42;
				this.jobb = this;
			}

			public Elem(Elem e, int t) // beszúrás e elem mögé
			{
				this.bal = e;
				this.tartalom = t;
				this.jobb = e.jobb;
				e.jobb.bal = this;
				e.jobb = this;
			}


		}
		static void Main(string[] args)
		{
			Elem fejelem = new Elem();


		}
	}
}
