using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicHanoi
{
    class Hanoi : ICloneable
    {
        int[] arr;
        int movecount = 0;
        int size = 0;
        int Parentindex = -1;
        int thisindex = 0;

        public Hanoi(int size)
        {
            this.size = size;
            arr = new int[size * 3];

            //첫번째 라인에 기본값 세팅, 나머지 라인도 0으로 세팅
            for (int i = 0; i < this.size; i++)
            {
                arr[i] = this.size - i;
            }
            for (int i = this.size; i < this.size * 3; i++)
            {
                arr[i] = 0;
            }
        }

        public bool equal(Hanoi b)
        {
            for (int i = 0; i < size * 3; i++)
            {
                if (b.arr[i] != arr[i])
                {
                    return false;
                }
            }
            return true;
        }


        //실제로 탑을 움직이고 기록 
        void moving(int from_index, int to_index)
        {
            movecount++;
            arr[to_index] = arr[from_index];
            arr[from_index] = 0;
        }

        //움직일 탑의 인덱스에서 목적지 라인으로 옮기려고 시도, 성공하면 true, 실패하면 false
        bool call_move(int no_index, int to)
        {
            int start = size * to - 1;
            int end = size * (to - 1);

            //목적지 라인에 탑이 있는지, 있으면 그 위로 탑을 옮길 수 있는지 살펴봄
            for (int i = start; i >= end; i--)
            {
                if (arr[i] != 0)
                {
                    if (arr[i] > arr[no_index])
                    {
                        moving(no_index, i + 1);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            //목적지 라인에 탑이 하나도 없으면 맨 밑으로 옮김
            moving(no_index, end);
            return true;
        }

        //출발지 라인에서 목적지 라인으로 탑을 옮기려고 시도, 성공했으면 true, 실패했으면 false
        public bool move(int from, int to)
        {
            int start = size * from - 1;
            int end = size * (from - 1);

            //탑의 위에서부터 살펴보면서 움직일 탑이 있는지 살펴봄
            //움직일 수 있으면 call_move 실행한 결과를 반환
            for (int i = start; i >= end; i--)
            {
                if (arr[i] != 0)
                {
                    return call_move(i, to);
                }
            }

            //움직일 탑이 없으면 false
            return false;
        }

        bool call_movetoanswer(int no_index, int to)
        {
            int start = size * to - 1;
            int end = size * (to - 1);

            //목적지 라인에 탑이 있는지, 있으면 그 위로 탑을 옮길 수 있는지 살펴봄
            for (int i = size - 1; i >= 0; i--)
            {
                int index = i + end;
                if (arr[index] != 0)
                {
                    if (arr[index] > arr[no_index])
                    {
                        if((arr[no_index] + size) % 2 == (i + to) % 2)
                        {
                            moving(no_index, index + 1);
                            return true;
                        }
                        return false;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            //목적지 라인에 탑이 하나도 없으면 맨 밑으로 옮김
            if ((arr[no_index] + size) % 2 == (to - 1) % 2)
            {
                moving(no_index, end);
                return true;
            }
            return false;
        }
        public bool move(int from)
        {
            int start = size * from - 1;
            int end = size * (from - 1);
            for (int i = start; i >= end; i--)
            {
                if(arr[i] == 0)
                {
                    continue;
                }
                if(arr[i] % 2 == size % 2)
                {
                    return call_movetoanswer(i, (from + 1) % 3 + 1);
                }
                else
                {
                    return call_movetoanswer(i, from % 3 + 1);
                }
            }
            return false;
        }

        //답지 만들어서 반환
        public Hanoi answer()
        {
            Hanoi new_hanoi = new Hanoi(size);
            for (int i = 0; i < size; i++)
            {
                new_hanoi.arr[i] = 0;
                new_hanoi.arr[size + i] = 0;
                new_hanoi.arr[size * 2 + i] = size - i;
            }

            return new_hanoi;
        }
        public int getmovecount() { return movecount; }
        public int gethash()
        {
            return arr[0] + arr[size] + arr[size * 2] - size;
            int h = 0;
            for (int i = 0;i < size; i++)
            {
                if(arr[i + size] != 0)
                {
                    h++;
                }
                else
                {
                    return h;
                }
                
            }
            return h;
        }
        public int getSize() { return size; }
        public int getScore()
        {
            int score = 0;
            for (int i = 0; i < size; i++)
            {
                if (arr[size * 2 + i] == size - i)
                {
                    score++;
                }
                else
                {
                    return score;
                }
            }
            return score;

            //int score = 0;
            //for(int i = 0; i < 3; i++)
            //{
            //    for(int j = size; j > 0; j--)
            //    {
            //        if(arr[i * size + j - 1] != 0)
            //        {
            //            score += j;
            //            break;
            //        }
            //    }
            //}
            //for(int i = 0;i < size; i++)
            //{
            //    if (arr[i] == size - i)
            //    {
            //        score--;
            //    }
            //    else
            //    {
            //        break;
            //    }
            //}
            //for(int i = 0; i < size; i++)
            //{
            //    if(arr[size * 2 + i] == size - i)
            //    {
            //        score++;
            //    }
            //    else
            //    {
            //        break;
            //    }
            //}
            //return score;
        }
        public bool gets()
        {
            for(int i = 0; i< 3; i++)
            {
                int value = arr[i * size];
                if (value < 2) continue;
                for(int j = 1; j < size; j++)
                {
                    int index = size * i + j;
                    if(--value != arr[index])
                    {
                        break;
                    }
                    if(value == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void pushindex(int thisi, int parenti)
        {
            thisindex = thisi;
            Parentindex = parenti;
        }
        public int getindex() { return thisindex; }

        public void getstack(Stack<int> st, Hanoilist glist)
        {
            st.Push(thisindex);
            if (Parentindex != -1)
                glist[Parentindex].getstack(st, glist);
        }

        public int[] backtracking(Hanoi Son)
        {
            int from = 0, to = 0;
            for(int i = 0;i < 3; i++)
            {
                for(int j = 0; j< size; j++)
                {
                    int index = i * size + j;
                    if(this.arr[index] != Son.arr[index])
                    {
                        if(Son.arr[index] == 0)
                        {
                            from = i;
                            j = size;
                        }
                        else
                        {
                            to = i;
                            j = size;
                        }
                    }
                }
            }

            return new int[2] { from + 1, to + 1 };
        }


        public object Clone()
        {
            Hanoi newhanoi = new Hanoi(size);
            newhanoi.arr = arr.Clone() as int[];
            newhanoi.movecount = this.movecount;
            newhanoi.Parentindex = Parentindex;
            newhanoi.thisindex = thisindex;
            return newhanoi;
        }
    }


    class Hanoilist : List<Hanoi>
    {
        List<int>[] list;
        int searchlimit;
        public int maxno = 0;

        public new void Add(Hanoi h)
        {
            int hash = h.gethash();
            int index = base.Count;
            base.Add(h);
            list[hash].Add(index);
        }

        public void setSize(int size)
        {
            maxno = 0;
            int listsize = (size - 1) * 2;
            //int listsize = size + 1;
            list = new List<int>[listsize];
            for (int i = 0; i < listsize; i++)
            {
                list[i] = new List<int>();
            }
            searchlimit = (int)Math.Pow(2, size);
        }

        public bool checkExist(Hanoi h)
        {
            int hash = h.gethash();
            
            for(int i = 0; i <list[hash].Count; i++)
            {
                //if (i > searchlimit) return false;
                int index = list[hash][list[hash].Count - i - 1];
                //if (Count - index > searchlimit) return false;
                if (h.equal(this[index]))
                {
                    if (this.Count - index > maxno) maxno = this.Count - index;
                    return true;
                }
            }
            return false;
        }

        
    }
}
