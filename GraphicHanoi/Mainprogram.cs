﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using WinFormlib;
using System.Diagnostics;

namespace GraphicHanoi
{
    class Mainprogram
    {
        ansHanoi Helper = null;
        Hanoi hanoi = null;
        Hanoi ans = null;
        Hanoilist glist = null;
        int size = 0;
        
        Form1 form;
        static TextViewer tv;
        static DoubleBuffering db;

        GraphicMap map;
        Stack<int> hindexstack = new Stack<int>();
        Stopwatch sw = new Stopwatch();

        static Queue<int> indexq = new Queue<int>();
        static bool playstoping = false;

        public Mainprogram(Form1 form)
        {
            db = DoubleBuffering.getinstance();
            db.setInstance(form);
            Timer_State.getinstance(form);
            this.form = form;
            tv = new TextViewer();

            Helper = new ansHanoi();
            map = new GraphicMap();
            GraphicMap.moveend += moveend;
        }

        public void setHanoi(int size)
        {
            this.size = size;
            hanoi = new Hanoi(size);
            glist = Helper.getGlist();
            tv.set(size, glist);
            
            tv.Startcal();
            Task t = Task.Factory.StartNew(() => {
                sw.Start();
                ans = Helper.get_answer(hanoi);
                sw.Stop();
                calend();
            });
            
        }
        void calend()
        {
            tv.Endcal(sw.ElapsedMilliseconds, ans.getmovecount());
            sw.Reset();
            form.calend();
        }

        public void playHanoi()
        {
            ans.getstack(hindexstack, glist);
            map.set(size);
            map.Show();

            
            int parent = hindexstack.Pop();

            while (hindexstack.Count != 0)
            {
                int son = hindexstack.Pop();
                indexq.Enqueue(son);

                int[] ft = glist[parent].backtracking(glist[son]);
                map.addmove(ft);
                parent = son;
            }
            tv.Startplay();
            map.startmove();
            form.playHanoi();
        }
        public void retry()
        {
            indexq.Clear();
            tv.retry();
            playstoping = false;
            try
            {
                map.unShow();
                tv.Endplay();
            }
            catch (Exception e)
            {

            }
            Helper.reset();
        }
        public void playpause(bool b)
        {
            playstoping = b;
        }
        public static bool nextmove()
        {
            while (playstoping) ;
            
            if (indexq.Count != 0)
                tv.nextmove(indexq.Dequeue());
            return true;
        }
        
        public void moveend()
        {
            form.showend();
        }
    }

    class TextViewer
    {
        DoubleBuffering db;
        Threading_Timer_v0 cal;
        Brush brush = new SolidBrush(Color.Black);
        Font font = new Font("Gulim", 20);
        Font f = new Font("Gulim", 13);

        int hanoisize;
        Hanoilist glist;
        bool caldrawing = false;
        bool playdrawing = false;

        const int plusdot = 100;
        const int dotnum = 3;
        int currentdotnum = 0;
        int count = 0;
        string str = "계산중";
        string timestr = "";
        static string indexstr = "";
        static int movecount = 0;


        public delegate void calculate();
        public static calculate calc;

        public TextViewer()
        {
            db = DoubleBuffering.getinstance();

            cal = new Threading_Timer_v0();
            cal.setInterval(10);
            cal.setCallback(() =>
            {
                if (calc != null)
                    calc();
            });
            cal.Start();

            
        }

        public void set(int size, Hanoilist list)
        {
            hanoisize = size;
            glist = list;
        }
        public void Startcal()
        {
            str = "계산중";
            timestr = "";
            calc += currentcount;
            calwrite();
        }

        public void Endcal(long time, int movecount)
        {
            str = "계산 완료 : " + hanoisize;
            timestr = "소요 시간 : " + time.ToString() + "ms";
            timestr += "\n Glist 크기 : " + glist.Count;
            timestr += "\n 이동 회수 : " + movecount;
            calc -= currentcount;
            
        }

        void currentcount()
        {
            timestr = "현재 Glist 크기 : " + glist.Count;
            if (++count > plusdot)
            {
                count = 0;
                if (++currentdotnum > dotnum)
                {
                    currentdotnum = 0;
                }
                str = hanoisize + " : 계산중";
                for (int i = 0; i < currentdotnum; i++)
                {
                    str += " .";
                }
            }
        }

        public void retry()
        {
            calunwrite();
            playunwrite();
            count = 0;
            movecount = 0;
        }

        public void Startplay()
        {
            calunwrite();
            playwrite();
        }

        public void Endplay()
        {
            playunwrite();
            

        }


        void calwrite()
        {
            if (caldrawing) return;
            caldrawing = true;
            db.callback_work += drawwait;
            db.callback_work += drawtime;
        }
        void calunwrite()
        {
            if (caldrawing)
            {
                caldrawing = false;
                db.callback_work -= drawwait;
                db.callback_work -= drawtime;
            }
        }
        void playwrite()
        {
            if (playdrawing) return;
            playdrawing = true;
            db.callback_work += drawindexno;
        }
        void playunwrite()
        {
            if (playdrawing)
            {
                playdrawing = false;
                db.callback_work -= drawindexno;
            }
        }

        public void nextmove(int index)
        {
            indexstr = "move : " + ++movecount + "\nindex : " + index;
        }



        void drawwait()
        {
            DoubleBuffering.getinstance().getGraphics.DrawString(str, font, brush, 270, 150);
        }
        void drawtime()
        {
            DoubleBuffering.getinstance().getGraphics.DrawString(timestr, f, brush, 270, 180);
        }
        void drawindexno()
        {
            DoubleBuffering.getinstance().getGraphics.DrawString(indexstr, f, brush, 550, 180);
        }
    }

    class ansHanoi
    {
        Queue<Hanoi> q = new Queue<Hanoi>();
        Hanoilist Glist = new Hanoilist();
        int maxscore = 0;

        public void reset()
        {
            Glist.Clear();
            q.Clear();
            maxscore = 0;
        }

        void InsertQueue(Hanoi h, Hanoi parent)
        {
            
            if (!Glist.checkExist(h))
            {
                int score = h.getScore();
                if (score < maxscore)
                {
                    return;
                }
                else if (score > maxscore)
                {
                    maxscore = score;
                }
                if (parent == null)
                {
                    h.pushindex(Glist.Count, -1);
                }
                else
                {
                    h.pushindex(Glist.Count, parent.getindex());
                }
                Glist.Add(h);
                
                q.Enqueue(h);
            }
        }

        public Hanoi get_answer(Hanoi h)
        {
            Glist.setSize(h.getSize());
            InsertQueue(h, null);

            Hanoi answer = h.answer();

            while(q.Count != 0)
            {
                Hanoi current = q.Dequeue();

                if (current.equal(answer))
                {
                    return current;
                }
                else
                {
                    Hanoi clone = current.Clone() as Hanoi;

                    for(int i = 1; i <= 3; i++)
                    {
                        for(int j = 1; j <=3; j++)
                        {
                            if(i == j)
                            {
                                continue;
                            }

                            if (clone.move(i, j))
                            {
                                InsertQueue(clone, current);
                                clone = current.Clone() as Hanoi;
                            }
                        }
                    }
                }
            }


            return null;
        }

        public Hanoilist getGlist() { return Glist; }
    }
}