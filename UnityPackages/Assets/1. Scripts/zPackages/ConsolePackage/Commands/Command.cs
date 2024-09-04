using System;

namespace ConsolePackage.Commands
{
	public class Command : AbstractCommand
	{
		private readonly Action callback;

		public Command(string name, Action commandCallback) : base(0)
		{
			Name     = name;
			callback = commandCallback;
		}

		public override string GetFullName()
		{
			return GetName();
		}

		public override void Invoke(params object[] parameters)
		{
			callback.Invoke();
		}
	}

	public class Command<TParam1> : AbstractCommand
	{
		private readonly Action<TParam1> callback;

		public Command(string name, Action<TParam1> commandCallback) : base(1)
		{
			Name     = name;
			callback = commandCallback;
		}

		public override string GetFullName()
		{
			return $"{GetName()} ({typeof(TParam1).Name})";
		}

		public override void Invoke(params object[] parameters)
		{
			if (!IsValidCast<TParam1>(parameters[0]))
			{
				return;
			}

			callback.Invoke(ConvertTo<TParam1>(parameters[0]));
		}
	}

	public class Command<TParam1, TParam2> : AbstractCommand
	{
		private readonly Action<TParam1, TParam2> callback;

		public Command(string name,
			Action<TParam1, TParam2>
				commandCallback)
			: base(2)
		{
			Name     = name;
			callback = commandCallback;
		}

		public override string GetFullName()
		{
			return $"{GetName()} ({typeof(TParam1).Name}, {typeof(TParam2).Name})";
		}

		public override void Invoke(params object[] parameters)
		{
			if (!IsValidCast<TParam1>(parameters[0]))
			{
				return;
			}

			if (!IsValidCast<TParam2>(parameters[1]))
			{
				return;
			}

			callback.Invoke
			(
				ConvertTo<TParam1>(parameters[0]),
				ConvertTo<TParam2>(parameters[1])
			);
		}
	}

	public class Command<TParam1, TParam2, TParam3> : AbstractCommand
	{
		private readonly Action<TParam1, TParam2, TParam3> callback;

		public Command(string name,
			Action<TParam1, TParam2, TParam3>
				commandCallback)
			: base(3)
		{
			Name     = name;
			callback = commandCallback;
		}

		public override string GetFullName()
		{
			return $"{GetName()} ({typeof(TParam1).Name}, {typeof(TParam2).Name}, {typeof(TParam3).Name})";
		}

		public override void Invoke(params object[] parameters)
		{
			if (!IsValidCast<TParam1>(parameters[0]))
			{
				return;
			}

			if (!IsValidCast<TParam2>(parameters[1]))
			{
				return;
			}

			if (!IsValidCast<TParam3>(parameters[2]))
			{
				return;
			}

			callback.Invoke
			(
				ConvertTo<TParam1>(parameters[0]),
				ConvertTo<TParam2>(parameters[1]),
				ConvertTo<TParam3>(parameters[2])
			);
		}
	}

	public class Command<TParam1, TParam2, TParam3, TParam4> : AbstractCommand
	{
		private readonly Action<TParam1, TParam2, TParam3, TParam4> callback;

		public Command(string name,
			Action<TParam1, TParam2, TParam3, TParam4>
				commandCallback)
			: base(4)
		{
			Name     = name;
			callback = commandCallback;
		}

		public override string GetFullName()
		{
			return
				$"{GetName()} ({typeof(TParam1).Name}, {typeof(TParam2).Name}, {typeof(TParam3).Name}, {typeof(TParam4).Name})";
		}

		public override void Invoke(params object[] parameters)
		{
			if (!IsValidCast<TParam1>(parameters[0]))
			{
				return;
			}

			if (!IsValidCast<TParam2>(parameters[1]))
			{
				return;
			}

			if (!IsValidCast<TParam3>(parameters[2]))
			{
				return;
			}

			if (!IsValidCast<TParam4>(parameters[3]))
			{
				return;
			}

			callback.Invoke
			(
				ConvertTo<TParam1>(parameters[0]),
				ConvertTo<TParam2>(parameters[1]),
				ConvertTo<TParam3>(parameters[2]),
				ConvertTo<TParam4>(parameters[3])
			);
		}
	}

	public class Command<TParam1, TParam2, TParam3, TParam4, TParam5> : AbstractCommand
	{
		private readonly Action<TParam1, TParam2, TParam3, TParam4, TParam5> callback;

		public Command(string name,
			Action<TParam1, TParam2, TParam3, TParam4, TParam5>
				commandCallback)
			: base(5)
		{
			Name     = name;
			callback = commandCallback;
		}

		public override string GetFullName()
		{
			return
				$"{GetName()} ({typeof(TParam1).Name}, {typeof(TParam2).Name}, {typeof(TParam3).Name}, {typeof(TParam4).Name}," +
				$" {typeof(TParam5).Name})";
		}

		public override void Invoke(params object[] parameters)
		{
			if (!IsValidCast<TParam1>(parameters[0]))
			{
				return;
			}

			if (!IsValidCast<TParam2>(parameters[1]))
			{
				return;
			}

			if (!IsValidCast<TParam3>(parameters[2]))
			{
				return;
			}

			if (!IsValidCast<TParam4>(parameters[3]))
			{
				return;
			}

			if (!IsValidCast<TParam5>(parameters[4]))
			{
				return;
			}

			callback.Invoke
			(
				ConvertTo<TParam1>(parameters[0]),
				ConvertTo<TParam2>(parameters[1]),
				ConvertTo<TParam3>(parameters[2]),
				ConvertTo<TParam4>(parameters[3]),
				ConvertTo<TParam5>(parameters[4])
			);
		}
	}

	public class Command<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6> : AbstractCommand
	{
		private readonly Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6> callback;

		public Command(string name,
			Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>
				commandCallback)
			: base(6)
		{
			Name     = name;
			callback = commandCallback;
		}

		public override string GetFullName()
		{
			return
				$"{GetName()} ({typeof(TParam1).Name}, {typeof(TParam2).Name}, {typeof(TParam3).Name}, {typeof(TParam4).Name}," +
				$" {typeof(TParam5).Name}, {typeof(TParam6).Name})";
		}

		public override void Invoke(params object[] parameters)
		{
			if (!IsValidCast<TParam1>(parameters[0]))
			{
				return;
			}

			if (!IsValidCast<TParam2>(parameters[1]))
			{
				return;
			}

			if (!IsValidCast<TParam3>(parameters[2]))
			{
				return;
			}

			if (!IsValidCast<TParam4>(parameters[3]))
			{
				return;
			}

			if (!IsValidCast<TParam5>(parameters[4]))
			{
				return;
			}

			if (!IsValidCast<TParam6>(parameters[5]))
			{
				return;
			}

			callback.Invoke
			(
				ConvertTo<TParam1>(parameters[0]),
				ConvertTo<TParam2>(parameters[1]),
				ConvertTo<TParam3>(parameters[2]),
				ConvertTo<TParam4>(parameters[3]),
				ConvertTo<TParam5>(parameters[4]),
				ConvertTo<TParam6>(parameters[5])
			);
		}
	}

	public class Command<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7> : AbstractCommand
	{
		private readonly Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7> callback;

		public Command(string name,
			Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7>
				commandCallback)
			: base(7)
		{
			Name     = name;
			callback = commandCallback;
		}

		public override string GetFullName()
		{
			return
				$"{GetName()} ({typeof(TParam1).Name}, {typeof(TParam2).Name}, {typeof(TParam3).Name}, {typeof(TParam4).Name}," +
				$" {typeof(TParam5).Name}, {typeof(TParam6).Name}, {typeof(TParam7).Name})";
		}

		public override void Invoke(params object[] parameters)
		{
			if (!IsValidCast<TParam1>(parameters[0]))
			{
				return;
			}

			if (!IsValidCast<TParam2>(parameters[1]))
			{
				return;
			}

			if (!IsValidCast<TParam3>(parameters[2]))
			{
				return;
			}

			if (!IsValidCast<TParam4>(parameters[3]))
			{
				return;
			}

			if (!IsValidCast<TParam5>(parameters[4]))
			{
				return;
			}

			if (!IsValidCast<TParam6>(parameters[5]))
			{
				return;
			}

			if (!IsValidCast<TParam7>(parameters[6]))
			{
				return;
			}

			callback.Invoke
			(
				ConvertTo<TParam1>(parameters[0]),
				ConvertTo<TParam2>(parameters[1]),
				ConvertTo<TParam3>(parameters[2]),
				ConvertTo<TParam4>(parameters[3]),
				ConvertTo<TParam5>(parameters[4]),
				ConvertTo<TParam6>(parameters[5]),
				ConvertTo<TParam7>(parameters[6])
			);
		}
	}

	public class Command<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8> : AbstractCommand
	{
		private readonly Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8> callback;

		public Command(string name,
			Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8>
				commandCallback)
			: base(8)
		{
			Name     = name;
			callback = commandCallback;
		}

		public override string GetFullName()
		{
			return
				$"{GetName()} ({typeof(TParam1).Name}, {typeof(TParam2).Name}, {typeof(TParam3).Name}, {typeof(TParam4).Name}," +
				$" {typeof(TParam5).Name}, {typeof(TParam6).Name}, {typeof(TParam7).Name}, {typeof(TParam8).Name})";
		}

		public override void Invoke(params object[] parameters)
		{
			if (!IsValidCast<TParam1>(parameters[0]))
			{
				return;
			}

			if (!IsValidCast<TParam2>(parameters[1]))
			{
				return;
			}

			if (!IsValidCast<TParam3>(parameters[2]))
			{
				return;
			}

			if (!IsValidCast<TParam4>(parameters[3]))
			{
				return;
			}

			if (!IsValidCast<TParam5>(parameters[4]))
			{
				return;
			}

			if (!IsValidCast<TParam6>(parameters[5]))
			{
				return;
			}

			if (!IsValidCast<TParam7>(parameters[6]))
			{
				return;
			}

			if (!IsValidCast<TParam8>(parameters[7]))
			{
				return;
			}

			callback.Invoke
			(
				ConvertTo<TParam1>(parameters[0]),
				ConvertTo<TParam2>(parameters[1]),
				ConvertTo<TParam3>(parameters[2]),
				ConvertTo<TParam4>(parameters[3]),
				ConvertTo<TParam5>(parameters[4]),
				ConvertTo<TParam6>(parameters[5]),
				ConvertTo<TParam7>(parameters[6]),
				ConvertTo<TParam8>(parameters[7])
			);
		}
	}

	public class Command<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8,
		TParam9> : AbstractCommand
	{
		private readonly Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8,
				TParam9>
			callback;

		public Command(string name,
			Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8,
					TParam9>
				commandCallback)
			: base(9)
		{
			Name     = name;
			callback = commandCallback;
		}

		public override string GetFullName()
		{
			return
				$"{GetName()} ({typeof(TParam1).Name}, {typeof(TParam2).Name}, {typeof(TParam3).Name}, {typeof(TParam4).Name}," +
				$" {typeof(TParam5).Name}, {typeof(TParam6).Name}, {typeof(TParam7).Name}, {typeof(TParam8).Name}," +
				$"{typeof(TParam9).Name})";
		}

		public override void Invoke(params object[] parameters)
		{
			if (!IsValidCast<TParam1>(parameters[0]))
			{
				return;
			}

			if (!IsValidCast<TParam2>(parameters[1]))
			{
				return;
			}

			if (!IsValidCast<TParam3>(parameters[2]))
			{
				return;
			}

			if (!IsValidCast<TParam4>(parameters[3]))
			{
				return;
			}

			if (!IsValidCast<TParam5>(parameters[4]))
			{
				return;
			}

			if (!IsValidCast<TParam6>(parameters[5]))
			{
				return;
			}

			if (!IsValidCast<TParam7>(parameters[6]))
			{
				return;
			}

			if (!IsValidCast<TParam8>(parameters[7]))
			{
				return;
			}

			if (!IsValidCast<TParam9>(parameters[8]))
			{
				return;
			}

			callback.Invoke
			(
				ConvertTo<TParam1>(parameters[0]),
				ConvertTo<TParam2>(parameters[1]),
				ConvertTo<TParam3>(parameters[2]),
				ConvertTo<TParam4>(parameters[3]),
				ConvertTo<TParam5>(parameters[4]),
				ConvertTo<TParam6>(parameters[5]),
				ConvertTo<TParam7>(parameters[6]),
				ConvertTo<TParam8>(parameters[7]),
				ConvertTo<TParam9>(parameters[8])
			);
		}
	}

	public class Command<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8,
		TParam9, TParam10> : AbstractCommand
	{
		private readonly
			Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8,
				TParam9, TParam10> callback;

		public Command(string name,
			Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8,
					TParam9, TParam10>
				commandCallback)
			: base(10)
		{
			Name     = name;
			callback = commandCallback;
		}

		public override string GetFullName()
		{
			return
				$"{GetName()} ({typeof(TParam1).Name}, {typeof(TParam2).Name}, {typeof(TParam3).Name}, {typeof(TParam4).Name}," +
				$" {typeof(TParam5).Name}, {typeof(TParam6).Name}, {typeof(TParam7).Name}, {typeof(TParam8).Name}," +
				$"{typeof(TParam9).Name}, {typeof(TParam10).Name})";
		}

		public override void Invoke(params object[] parameters)
		{
			if (!IsValidCast<TParam1>(parameters[0]))
			{
				return;
			}

			if (!IsValidCast<TParam2>(parameters[1]))
			{
				return;
			}

			if (!IsValidCast<TParam3>(parameters[2]))
			{
				return;
			}

			if (!IsValidCast<TParam4>(parameters[3]))
			{
				return;
			}

			if (!IsValidCast<TParam5>(parameters[4]))
			{
				return;
			}

			if (!IsValidCast<TParam6>(parameters[5]))
			{
				return;
			}

			if (!IsValidCast<TParam7>(parameters[6]))
			{
				return;
			}

			if (!IsValidCast<TParam8>(parameters[7]))
			{
				return;
			}

			if (!IsValidCast<TParam9>(parameters[8]))
			{
				return;
			}

			if (!IsValidCast<TParam10>(parameters[9]))
			{
				return;
			}

			callback.Invoke
			(
				ConvertTo<TParam1>(parameters[0]),
				ConvertTo<TParam2>(parameters[1]),
				ConvertTo<TParam3>(parameters[2]),
				ConvertTo<TParam4>(parameters[3]),
				ConvertTo<TParam5>(parameters[4]),
				ConvertTo<TParam6>(parameters[5]),
				ConvertTo<TParam7>(parameters[6]),
				ConvertTo<TParam8>(parameters[7]),
				ConvertTo<TParam9>(parameters[8]),
				ConvertTo<TParam10>(parameters[9])
			);
		}
	}

	public class Command<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8,
		TParam9, TParam10, TParam11> : AbstractCommand
	{
		private readonly Action<TParam1, TParam2, TParam3, TParam4,
			TParam5, TParam6, TParam7, TParam8,
			TParam9, TParam10, TParam11> callback;

		public Command(string name,
			Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8,
					TParam9, TParam10, TParam11>
				commandCallback)
			: base(11)
		{
			Name     = name;
			callback = commandCallback;
		}

		public override string GetFullName()
		{
			return
				$"{GetName()} ({typeof(TParam1).Name}, {typeof(TParam2).Name}, {typeof(TParam3).Name}, {typeof(TParam4).Name}," +
				$" {typeof(TParam5).Name}, {typeof(TParam6).Name}, {typeof(TParam7).Name}, {typeof(TParam8).Name}," +
				$"{typeof(TParam9).Name}, {typeof(TParam10).Name}, {typeof(TParam11).Name})";
		}

		public override void Invoke(params object[] parameters)
		{
			if (!IsValidCast<TParam1>(parameters[0]))
			{
				return;
			}

			if (!IsValidCast<TParam2>(parameters[1]))
			{
				return;
			}

			if (!IsValidCast<TParam3>(parameters[2]))
			{
				return;
			}

			if (!IsValidCast<TParam4>(parameters[3]))
			{
				return;
			}

			if (!IsValidCast<TParam5>(parameters[4]))
			{
				return;
			}

			if (!IsValidCast<TParam6>(parameters[5]))
			{
				return;
			}

			if (!IsValidCast<TParam7>(parameters[6]))
			{
				return;
			}

			if (!IsValidCast<TParam8>(parameters[7]))
			{
				return;
			}

			if (!IsValidCast<TParam9>(parameters[8]))
			{
				return;
			}

			if (!IsValidCast<TParam10>(parameters[9]))
			{
				return;
			}

			if (!IsValidCast<TParam11>(parameters[10]))
			{
				return;
			}

			callback.Invoke
			(
				ConvertTo<TParam1>(parameters[0]),
				ConvertTo<TParam2>(parameters[1]),
				ConvertTo<TParam3>(parameters[2]),
				ConvertTo<TParam4>(parameters[3]),
				ConvertTo<TParam5>(parameters[4]),
				ConvertTo<TParam6>(parameters[5]),
				ConvertTo<TParam7>(parameters[6]),
				ConvertTo<TParam8>(parameters[7]),
				ConvertTo<TParam9>(parameters[8]),
				ConvertTo<TParam10>(parameters[9]),
				ConvertTo<TParam11>(parameters[10])
			);
		}
	}

	public class Command<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8,
		TParam9, TParam10, TParam11, TParam12> : AbstractCommand
	{
		private readonly Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8,
			TParam9, TParam10, TParam11, TParam12> callback;

		public Command(string name,
			Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8,
					TParam9, TParam10, TParam11, TParam12>
				commandCallback)
			: base(12)
		{
			Name     = name;
			callback = commandCallback;
		}

		public override string GetFullName()
		{
			return
				$"{GetName()} ({typeof(TParam1).Name}, {typeof(TParam2).Name}, {typeof(TParam3).Name}, {typeof(TParam4).Name}," +
				$" {typeof(TParam5).Name}, {typeof(TParam6).Name}, {typeof(TParam7).Name}, {typeof(TParam8).Name}," +
				$"{typeof(TParam9).Name}, {typeof(TParam10).Name}, {typeof(TParam11).Name}, {typeof(TParam12).Name})";
		}

		public override void Invoke(params object[] parameters)
		{
			if (!IsValidCast<TParam1>(parameters[0]))
			{
				return;
			}

			if (!IsValidCast<TParam2>(parameters[1]))
			{
				return;
			}

			if (!IsValidCast<TParam3>(parameters[2]))
			{
				return;
			}

			if (!IsValidCast<TParam4>(parameters[3]))
			{
				return;
			}

			if (!IsValidCast<TParam5>(parameters[4]))
			{
				return;
			}

			if (!IsValidCast<TParam6>(parameters[5]))
			{
				return;
			}

			if (!IsValidCast<TParam7>(parameters[6]))
			{
				return;
			}

			if (!IsValidCast<TParam8>(parameters[7]))
			{
				return;
			}

			if (!IsValidCast<TParam9>(parameters[8]))
			{
				return;
			}

			if (!IsValidCast<TParam10>(parameters[9]))
			{
				return;
			}

			if (!IsValidCast<TParam11>(parameters[10]))
			{
				return;
			}

			if (!IsValidCast<TParam12>(parameters[11]))
			{
				return;
			}

			callback.Invoke
			(
				ConvertTo<TParam1>(parameters[0]),
				ConvertTo<TParam2>(parameters[1]),
				ConvertTo<TParam3>(parameters[2]),
				ConvertTo<TParam4>(parameters[3]),
				ConvertTo<TParam5>(parameters[4]),
				ConvertTo<TParam6>(parameters[5]),
				ConvertTo<TParam7>(parameters[6]),
				ConvertTo<TParam8>(parameters[7]),
				ConvertTo<TParam9>(parameters[8]),
				ConvertTo<TParam10>(parameters[9]),
				ConvertTo<TParam11>(parameters[10]),
				ConvertTo<TParam12>(parameters[11])
			);
		}
	}

	public class Command<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8,
		TParam9, TParam10, TParam11, TParam12, TParam13> : AbstractCommand
	{
		private readonly Action<
			TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8,
			TParam9, TParam10, TParam11, TParam12, TParam13> callback;

		public Command(string name,
			Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8,
					TParam9, TParam10, TParam11, TParam12, TParam13>
				commandCallback)
			: base(13)
		{
			Name     = name;
			callback = commandCallback;
		}

		public override string GetFullName()
		{
			return
				$"{GetName()} ({typeof(TParam1).Name}, {typeof(TParam2).Name}, {typeof(TParam3).Name}, {typeof(TParam4).Name}," +
				$" {typeof(TParam5).Name}, {typeof(TParam6).Name}, {typeof(TParam7).Name}, {typeof(TParam8).Name}," +
				$"{typeof(TParam9).Name}, {typeof(TParam10).Name}, {typeof(TParam11).Name}, {typeof(TParam12).Name}," +
				$"{typeof(TParam13).Name})";
		}

		public override void Invoke(params object[] parameters)
		{
			if (!IsValidCast<TParam1>(parameters[0]))
			{
				return;
			}

			if (!IsValidCast<TParam2>(parameters[1]))
			{
				return;
			}

			if (!IsValidCast<TParam3>(parameters[2]))
			{
				return;
			}

			if (!IsValidCast<TParam4>(parameters[3]))
			{
				return;
			}

			if (!IsValidCast<TParam5>(parameters[4]))
			{
				return;
			}

			if (!IsValidCast<TParam6>(parameters[5]))
			{
				return;
			}

			if (!IsValidCast<TParam7>(parameters[6]))
			{
				return;
			}

			if (!IsValidCast<TParam8>(parameters[7]))
			{
				return;
			}

			if (!IsValidCast<TParam9>(parameters[8]))
			{
				return;
			}

			if (!IsValidCast<TParam10>(parameters[9]))
			{
				return;
			}

			if (!IsValidCast<TParam11>(parameters[10]))
			{
				return;
			}

			if (!IsValidCast<TParam12>(parameters[11]))
			{
				return;
			}

			if (!IsValidCast<TParam13>(parameters[12]))
			{
				return;
			}

			callback.Invoke
			(
				ConvertTo<TParam1>(parameters[0]),
				ConvertTo<TParam2>(parameters[1]),
				ConvertTo<TParam3>(parameters[2]),
				ConvertTo<TParam4>(parameters[3]),
				ConvertTo<TParam5>(parameters[4]),
				ConvertTo<TParam6>(parameters[5]),
				ConvertTo<TParam7>(parameters[6]),
				ConvertTo<TParam8>(parameters[7]),
				ConvertTo<TParam9>(parameters[8]),
				ConvertTo<TParam10>(parameters[9]),
				ConvertTo<TParam11>(parameters[10]),
				ConvertTo<TParam12>(parameters[11]),
				ConvertTo<TParam13>(parameters[12])
			);
		}
	}

	public class Command<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8,
		TParam9, TParam10, TParam11, TParam12, TParam13, TParam14> : AbstractCommand
	{
		private readonly Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8,
			TParam9, TParam10, TParam11, TParam12, TParam13, TParam14> callback;

		public Command(string name,
			Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8,
					TParam9, TParam10, TParam11, TParam12, TParam13, TParam14>
				commandCallback)
			: base(14)
		{
			Name     = name;
			callback = commandCallback;
		}

		public override string GetFullName()
		{
			return
				$"{GetName()} ({typeof(TParam1).Name}, {typeof(TParam2).Name}, {typeof(TParam3).Name}, {typeof(TParam4).Name}," +
				$" {typeof(TParam5).Name}, {typeof(TParam6).Name}, {typeof(TParam7).Name}, {typeof(TParam8).Name}," +
				$"{typeof(TParam9).Name}, {typeof(TParam10).Name}, {typeof(TParam11).Name}, {typeof(TParam12).Name}," +
				$"{typeof(TParam13).Name}, {typeof(TParam14).Name})";
		}

		public override void Invoke(params object[] parameters)
		{
			if (!IsValidCast<TParam1>(parameters[0]))
			{
				return;
			}

			if (!IsValidCast<TParam2>(parameters[1]))
			{
				return;
			}

			if (!IsValidCast<TParam3>(parameters[2]))
			{
				return;
			}

			if (!IsValidCast<TParam4>(parameters[3]))
			{
				return;
			}

			if (!IsValidCast<TParam5>(parameters[4]))
			{
				return;
			}

			if (!IsValidCast<TParam6>(parameters[5]))
			{
				return;
			}

			if (!IsValidCast<TParam7>(parameters[6]))
			{
				return;
			}

			if (!IsValidCast<TParam8>(parameters[7]))
			{
				return;
			}

			if (!IsValidCast<TParam9>(parameters[8]))
			{
				return;
			}

			if (!IsValidCast<TParam10>(parameters[9]))
			{
				return;
			}

			if (!IsValidCast<TParam11>(parameters[10]))
			{
				return;
			}

			if (!IsValidCast<TParam12>(parameters[11]))
			{
				return;
			}

			if (!IsValidCast<TParam13>(parameters[12]))
			{
				return;
			}

			if (!IsValidCast<TParam14>(parameters[13]))
			{
				return;
			}

			callback.Invoke
			(
				ConvertTo<TParam1>(parameters[0]),
				ConvertTo<TParam2>(parameters[1]),
				ConvertTo<TParam3>(parameters[2]),
				ConvertTo<TParam4>(parameters[3]),
				ConvertTo<TParam5>(parameters[4]),
				ConvertTo<TParam6>(parameters[5]),
				ConvertTo<TParam7>(parameters[6]),
				ConvertTo<TParam8>(parameters[7]),
				ConvertTo<TParam9>(parameters[8]),
				ConvertTo<TParam10>(parameters[9]),
				ConvertTo<TParam11>(parameters[10]),
				ConvertTo<TParam12>(parameters[11]),
				ConvertTo<TParam13>(parameters[12]),
				ConvertTo<TParam14>(parameters[13])
			);
		}
	}

	public class Command<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8,
		TParam9, TParam10, TParam11, TParam12, TParam13, TParam14, TParam15> : AbstractCommand
	{
		private readonly Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8,
			TParam9, TParam10, TParam11, TParam12, TParam13, TParam14, TParam15> callback;

		public Command(string name,
			Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8,
					TParam9, TParam10, TParam11, TParam12, TParam13, TParam14, TParam15>
				commandCallback)
			: base(15)
		{
			Name     = name;
			callback = commandCallback;
		}

		public override string GetFullName()
		{
			return
				$"{GetName()} ({typeof(TParam1).Name}, {typeof(TParam2).Name}, {typeof(TParam3).Name}, {typeof(TParam4).Name}," +
				$" {typeof(TParam5).Name}, {typeof(TParam6).Name}, {typeof(TParam7).Name}, {typeof(TParam8).Name}," +
				$"{typeof(TParam9).Name}, {typeof(TParam10).Name}, {typeof(TParam11).Name}, {typeof(TParam12).Name}," +
				$"{typeof(TParam13).Name}, {typeof(TParam14).Name}, {typeof(TParam15).Name})";
		}

		public override void Invoke(params object[] parameters)
		{
			if (!IsValidCast<TParam1>(parameters[0]))
			{
				return;
			}

			if (!IsValidCast<TParam2>(parameters[1]))
			{
				return;
			}

			if (!IsValidCast<TParam3>(parameters[2]))
			{
				return;
			}

			if (!IsValidCast<TParam4>(parameters[3]))
			{
				return;
			}

			if (!IsValidCast<TParam5>(parameters[4]))
			{
				return;
			}

			if (!IsValidCast<TParam6>(parameters[5]))
			{
				return;
			}

			if (!IsValidCast<TParam7>(parameters[6]))
			{
				return;
			}

			if (!IsValidCast<TParam8>(parameters[7]))
			{
				return;
			}

			if (!IsValidCast<TParam9>(parameters[8]))
			{
				return;
			}

			if (!IsValidCast<TParam10>(parameters[9]))
			{
				return;
			}

			if (!IsValidCast<TParam11>(parameters[10]))
			{
				return;
			}

			if (!IsValidCast<TParam12>(parameters[11]))
			{
				return;
			}

			if (!IsValidCast<TParam13>(parameters[12]))
			{
				return;
			}

			if (!IsValidCast<TParam14>(parameters[13]))
			{
				return;
			}

			if (!IsValidCast<TParam15>(parameters[14]))
			{
				return;
			}

			callback.Invoke
			(
				ConvertTo<TParam1>(parameters[0]),
				ConvertTo<TParam2>(parameters[1]),
				ConvertTo<TParam3>(parameters[2]),
				ConvertTo<TParam4>(parameters[3]),
				ConvertTo<TParam5>(parameters[4]),
				ConvertTo<TParam6>(parameters[5]),
				ConvertTo<TParam7>(parameters[6]),
				ConvertTo<TParam8>(parameters[7]),
				ConvertTo<TParam9>(parameters[8]),
				ConvertTo<TParam10>(parameters[9]),
				ConvertTo<TParam11>(parameters[10]),
				ConvertTo<TParam12>(parameters[11]),
				ConvertTo<TParam13>(parameters[12]),
				ConvertTo<TParam14>(parameters[13]),
				ConvertTo<TParam15>(parameters[14])
			);
		}
	}

	public class Command<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8,
		TParam9, TParam10, TParam11, TParam12, TParam13, TParam14, TParam15, TParam16> : AbstractCommand
	{
		private readonly Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8,
			TParam9, TParam10, TParam11, TParam12, TParam13, TParam14, TParam15, TParam16> callback;

		public Command(string name,
			Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8,
					TParam9, TParam10, TParam11, TParam12, TParam13, TParam14, TParam15, TParam16>
				commandCallback)
			: base(16)
		{
			Name     = name;
			callback = commandCallback;
		}

		public override string GetFullName()
		{
			return
				$"{GetName()} ({typeof(TParam1).Name}, {typeof(TParam2).Name}, {typeof(TParam3).Name}, {typeof(TParam4).Name}," +
				$" {typeof(TParam5).Name}, {typeof(TParam6).Name}, {typeof(TParam7).Name}, {typeof(TParam8).Name}," +
				$"{typeof(TParam9).Name}, {typeof(TParam10).Name}, {typeof(TParam11).Name}, {typeof(TParam12).Name}," +
				$"{typeof(TParam13).Name}, {typeof(TParam14).Name}, {typeof(TParam15).Name}, {typeof(TParam16).Name})";
		}

		public override void Invoke(params object[] parameters)
		{
			if (!IsValidCast<TParam1>(parameters[0]))
			{
				return;
			}

			if (!IsValidCast<TParam2>(parameters[1]))
			{
				return;
			}

			if (!IsValidCast<TParam3>(parameters[2]))
			{
				return;
			}

			if (!IsValidCast<TParam4>(parameters[3]))
			{
				return;
			}

			if (!IsValidCast<TParam5>(parameters[4]))
			{
				return;
			}

			if (!IsValidCast<TParam6>(parameters[5]))
			{
				return;
			}

			if (!IsValidCast<TParam7>(parameters[6]))
			{
				return;
			}

			if (!IsValidCast<TParam8>(parameters[7]))
			{
				return;
			}

			if (!IsValidCast<TParam9>(parameters[8]))
			{
				return;
			}

			if (!IsValidCast<TParam10>(parameters[9]))
			{
				return;
			}

			if (!IsValidCast<TParam11>(parameters[10]))
			{
				return;
			}

			if (!IsValidCast<TParam12>(parameters[11]))
			{
				return;
			}

			if (!IsValidCast<TParam13>(parameters[12]))
			{
				return;
			}

			if (!IsValidCast<TParam14>(parameters[13]))
			{
				return;
			}

			if (!IsValidCast<TParam15>(parameters[14]))
			{
				return;
			}

			if (!IsValidCast<TParam16>(parameters[15]))
			{
				return;
			}

			callback.Invoke
			(
				ConvertTo<TParam1>(parameters[0]),
				ConvertTo<TParam2>(parameters[1]),
				ConvertTo<TParam3>(parameters[2]),
				ConvertTo<TParam4>(parameters[3]),
				ConvertTo<TParam5>(parameters[4]),
				ConvertTo<TParam6>(parameters[5]),
				ConvertTo<TParam7>(parameters[6]),
				ConvertTo<TParam8>(parameters[7]),
				ConvertTo<TParam9>(parameters[8]),
				ConvertTo<TParam10>(parameters[9]),
				ConvertTo<TParam11>(parameters[10]),
				ConvertTo<TParam12>(parameters[11]),
				ConvertTo<TParam13>(parameters[12]),
				ConvertTo<TParam14>(parameters[13]),
				ConvertTo<TParam15>(parameters[14]),
				ConvertTo<TParam16>(parameters[15])
			);
		}
	}
}