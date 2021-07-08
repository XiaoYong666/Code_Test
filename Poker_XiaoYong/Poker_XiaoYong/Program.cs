using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Poker_XiaoYong
{
	class Program
	{
		static	Dictionary<int, List<int>> chooseAll = new Dictionary<int, List<int>>();
		static	Random _random= new Random();
		static object obj = new object();
		static  void Main(string[] args)
		{
			Console.WriteLine("输入【开始】即刻体验游戏～");
			var conRead=Console.ReadLine();
			if (conRead == "开始")
			{
				Console.WriteLine("输入成功，开始游戏");
				GetAllData();
				GetTakePoker();

			}
			else
			{
				Console.WriteLine("输入有误，游戏结束～");
			}

		}

		/// <summary>
		/// 初始化游戏数据
		/// </summary>
		private  static void GetAllData()
		{
			chooseAll?.Clear();
			chooseAll.Add(1, new List<int>() { 1, 2, 3 });
			chooseAll.Add(2, new List<int>() { 4, 5, 6, 7, 8 });
			chooseAll.Add(3, new List<int>() { 9, 10, 11, 12, 13, 14, 15 });
		}
		
		/// <summary>
		/// 拿火柴
		/// </summary>
		private  async static void GetTakePoker()
		{
			var userSelectedRow = _random.Next(1, chooseAll.Count + 1);
			if(chooseAll[userSelectedRow]==null || chooseAll[userSelectedRow].Count == 0)
			{
				Console.WriteLine("所提供的源数据异常");
				return;
			}

			while (chooseAll[userSelectedRow].Count > 0)
			{
				Console.WriteLine($"用户1开始拿第{userSelectedRow}行火柴");
				Console.WriteLine($"用户2开始拿第{userSelectedRow}行火柴");
				var taskList = new List<Task<bool>> { userOneTake(userSelectedRow), userTwoTake(userSelectedRow) };
				await Task.WhenAll(taskList);
			}
		}

		/// <summary>
		/// 用户一拿火柴
		/// </summary>
		/// <param name="userOneSelectedRow"></param>
		/// <returns></returns>
		private static Task<bool> userOneTake(int userOneSelectedRow)
		{
			return Task.Run(() =>
			{
				try
				{
					GetPokerCaculate(userOneSelectedRow, Operation.userOne);
					return true;
				}
				catch (Exception ex)
				{
					Console.WriteLine($"发生异常：{ex.Message}");
					return false;
				}
			});
		}
	
		/// <summary>
		/// 用户二拿火柴
		/// </summary>
		/// <param name="userOneSelectedRow"></param>
		/// <returns></returns>
		private static Task<bool> userTwoTake(int userOneSelectedRow)
		{
			return Task.Run(() =>
			{
				try
				{
					GetPokerCaculate(userOneSelectedRow, Operation.userTwo);
					return true;
				}
				catch (Exception ex)
				{
					Console.WriteLine($"发生异常：{ex.Message}");
					return false;
				}
			});
		}

		/// <summary>
		/// 取火柴计算
		/// </summary>
		/// <param name="rowIndex"></param>
		/// <param name="operation"></param>
		private static void GetPokerCaculate(int rowIndex, Operation operation)
		{
			lock (obj)
			{
				List<int> lstSelectedRowData = chooseAll[rowIndex];
				if (lstSelectedRowData.Count == 0)
				{
					return;
				}
				int selectCount = _random.Next(1, lstSelectedRowData.Count + 1);
				Console.WriteLine($"取走{selectCount}根火柴");
				chooseAll[rowIndex].RemoveRange(0, selectCount);
				if (chooseAll[rowIndex].Count == 0)//最后一个取完列表中的火柴判定为输
				{
					string content = string.Empty;
					if (operation == Operation.userOne)
					{
						content = "userOne lose";
					}
					else
					{
						content = "userTwo lose";
					}
					Console.WriteLine(content);
				}
			}
		}

		/// <summary>
		/// 操作人枚举
		/// </summary>
		public enum Operation
		{
			userOne,
			userTwo
		}
	}
}
