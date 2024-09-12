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

			public class Elem
			{
				public Elem bal;
				public int tartalom;
				public Elem jobb;

				Elem(Elem bal, int tartalom, Elem jobb) // ide vissza lehetne vezetni a többi konstruktort!
				{
					this.bal = bal;
					this.tartalom = tartalom;
					this.jobb = jobb;
				}

				public Elem() 
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

				public void Töröl()
				{
					this.bal.jobb = this.jobb;
					this.jobb.bal = this.bal;
				}

			}

			Elem fejelem;
			public int Count { get; private set; }


			public LancoltLista()
			{
				this.fejelem = new Elem();
				this.Count = 0;
			}

			public void Add(int x) 
			{
				new Elem(Utolso(), x);
				Count++;
			}

			public Elem Utolso()
			{
				return fejelem.bal;
			}


			public Elem Keres(int x)
			{
				Elem i = fejelem.jobb;
				while (i!= fejelem && i.tartalom != x)
				{
					i = i.jobb;
				}
				return i != fejelem ? i : null;
			}

			public void Remove(int x)
			{
				Elem torlendo = Keres(x);
				if (torlendo != null)
					torlendo.Töröl();
				Count--;
			}

			public Elem At(int index)
			{
				if (Count <= index || index<0)
					throw new IndexOutOfRangeException();

				Elem i = fejelem.jobb;
				for (int szamlalo = 0; szamlalo < index; szamlalo++)
					i = i.jobb;

				return i;
			}

			public int this[int index]
			{
				get // "= jel jobb oldalán van"
				{
					return At(index).tartalom;
				}
				set // "= jel bal oldalán van"
				{
					At(index).tartalom = value;
				}
			}

			public void RemoveAt(int i)
			{
				At(i).Töröl();
				Count--;
			}


			public void Diagnosztika()
			{
				string s = "F = ";

				Elem i = fejelem.jobb; // mint iterátor!

				while (i != fejelem)
				{
					s += $"{i.tartalom} = ";
					i = i.jobb;
				}

				s += "F";

                Console.WriteLine(s);
            }
		}

		static void Main(string[] args)
		{
			LancoltLista l = new LancoltLista();

			l.Add(2);
			l.Add(3);
			l.Add(7);
            l.Remove(2);
            // l.RemoveAt(0);

            Console.WriteLine(l);
			l.Diagnosztika();

			l.Add(5);
			l.Diagnosztika();
			l.Add(9);
			l.Diagnosztika();
			l.Add(9);
			l.Diagnosztika();

			l.RemoveAt(4);
			l.Diagnosztika();

			Console.WriteLine(l.Count);
			// l.Count = -3; // ez nem működik, és ez így is helyes!
			l.Diagnosztika();
			Console.WriteLine(l[2]);
			l.Diagnosztika();

			l[2] = 12;
			l.Diagnosztika();

		}
	}
}
