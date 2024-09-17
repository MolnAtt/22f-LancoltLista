using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LancoltVerem
{
	internal class Program
	{

		class LancoltLista<T>
		{
			public class Elem<T>
			{
				public Elem<T> bal;
				public T tartalom;
				public Elem<T> jobb;

				Elem(Elem<T> bal, T tartalom, Elem<T> jobb) // ide vissza lehetne vezetni a többi konstruktort!
				{
					this.bal = bal;
					this.tartalom = tartalom;
					this.jobb = jobb;
				}

				public Elem()
				{
					this.bal = this;
					this.tartalom = default;
					this.jobb = this;
				}

				public Elem(Elem<T> e, T t) // beszúrás e elem mögé
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
			Elem<T> fejelem;
			public int Count { get; private set; }
			public LancoltLista()
			{
				this.fejelem = new Elem<T>();
				this.Count = 0;
			}
			public void Add(T x)
			{
				new Elem<T>(Utolso(), x);
				Count++;
			}
			Elem<T> Utolso() => fejelem.bal;
			Elem<T> Keres(T x)
			{
				Elem<T> i = fejelem.jobb;
				while (i != fejelem && !i.tartalom.Equals(x))
				{
					i = i.jobb;
				}
				return i != fejelem ? i : null;
			}
			public void Remove(T x)
			{
				Elem<T> torlendo = Keres(x);
				if (torlendo != null)
				{
					torlendo.Töröl();
					Count--;
				}
			}
			Elem<T> At(int index)
			{
				if (Count <= index || index < 0)
					throw new IndexOutOfRangeException();

				Elem<T> i = fejelem.jobb;
				for (int szamlalo = 0; szamlalo < index; szamlalo++)
					i = i.jobb;

				return i;
			}
			public T this[int index]
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

				Elem<T> i = fejelem.jobb; // mint iterátor!

				while (i != fejelem)
				{
					s += $"{i.tartalom} = ";
					i = i.jobb;
				}

				s += "F";

				Console.WriteLine(s);
			}
			public T Max(Func<T, T, int> comparator)
			{
				if (Count == 0)
					throw new IndexOutOfRangeException();

				Elem<T> i = fejelem.jobb;
				T best = i.tartalom;
				while (i != fejelem)
				{
					if (comparator(best, i.tartalom) == -1)
					{
						best = i.tartalom;
					}
					i = i.jobb;
				}
				return best;
			}

			public LancoltLista<T> Where(Func<T, bool> predicate)
			{
				LancoltLista<T> result = new LancoltLista<T>();

				Elem<T> i = fejelem.jobb;

				while (i != fejelem) 
				{
					if (predicate(i.tartalom))
					{
						result.Add(i.tartalom);
					}
				}

				return result;
			}

			public LancoltLista<S> Select<S>(Func<T, S> selector)
			{
				LancoltLista<S> result = new LancoltLista<S>();

				Elem<T> i = fejelem.jobb;

				while (i != fejelem)
				{
					result.Add(selector(i.tartalom));
					i = i.jobb;
				}

				return result;
			}

			public int FindIndex(Func<T,bool> predicate)
			{
				int index = 0;
				Elem<T> i = fejelem.jobb;
				while (i != fejelem && !predicate(i.tartalom))
				{
					i = i.jobb;
					index++;
				}
				return i != fejelem ? index : -1;
			}
			public int FindLastIndex(Func<T, bool> predicate)
			{
				int index = Count-1;
				Elem<T> i = fejelem.bal;
				while (i != fejelem && !predicate(i.tartalom))
				{
					i = i.bal;
					index--;
				}
				return i != fejelem ? index : -1;
			}
			public T First(Func<T, bool> predicate)
			{
				Elem<T> i = fejelem.jobb;
				while (i != fejelem && !predicate(i.tartalom))
				{
					i = i.jobb;
				}
				if (i == fejelem)
					throw new Exception("Ilyen tulajdonságú elem nincs a listában");

				return i.tartalom;
			}
			public T First()
			{
				if (Count == 0)
					throw new Exception("Nincs is elem a listában!");
				return fejelem.jobb.tartalom;
			}
			public T Last()
			{
				if (Count == 0)
					throw new Exception("Nincs is elem a listában!");
				return fejelem.bal.tartalom;
			}
			public T Last(Func<T, bool> predicate)
			{
				Elem<T> i = fejelem.bal;
				while (i != fejelem && !predicate(i.tartalom))
				{
					i = i.bal;
				}
				if (i == fejelem)
					throw new Exception("Ilyen tulajdonságú elem nincs a listában");

				return i.tartalom;
			}
			public bool Contains(Func<T, bool> predicate)
			{
				Elem<T> i = fejelem.jobb;
				while (i != fejelem && !predicate(i.tartalom))
				{
					i = i.jobb;
				}
				return i != fejelem;
			}
			public bool Contains(T elem) => Keres(elem) != null;
			//public bool Contains(T elem) => Contains(x=> x.Equals(elem));

		}

		static void Main(string[] args)
		{
			LancoltLista<string> l = new LancoltLista<string>();

			l.Add("a");
			l.Add("asvd");
			l.Add("ab");
			l.Add("1wer");
			l.Add("5aasdf");
			l.Add("aasdfdf");
			l.Diagnosztika();

			Console.WriteLine(l.Max((s, t) => s.Length < t.Length ? -1 : (s.Length == t.Length ? 0 : 1)));
			Console.WriteLine(l.Max((s, t) => s.CompareTo(t)));
			//l.Select(s => "- " + s).Diagnosztika();

            Console.WriteLine(l.First(x => x.Length==2));

        }
	}
}
