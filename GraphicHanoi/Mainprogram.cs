using System;
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
        bool mode = false;

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
            map = new GraphicMap(form);
            GraphicMap.moveend += moveend;
        }

        public void form_sizechange()
        {
            Size s = form.ClientSize;
            tv.setSize(s);
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
                try
                {
                    ans = Helper.get_answer(hanoi, mode);
                }
                catch(Exception e)
                {
                    
                }
                
                sw.Stop();
                calend();
            });
            
        }
        void calend()
        {
            if(ans == null)
            {
                tv.Endcal(sw.ElapsedMilliseconds, 0);
            }
            else
            {
                tv.Endcal(sw.ElapsedMilliseconds, ans.getmovecount());
            }
            int maxno = glist.maxno;
            sw.Reset();
            form.calend();
        }

        public bool modechange()
        {
            mode = !mode;
            return mode;
        }
        public void playHanoi()
        {
            map.set(size);
            map.Show();

            if (mode)
            {
                int arrsize = ans.getmovecount() + 1;
                int parent = 0;
                int son = parent;
                while(++son < arrsize)
                {
                    indexq.Enqueue(son);
                    short[] ft = glist[parent].backtracking(glist[son]);
                    map.addmove(ft);
                    parent = son;
                }
            }
            else
            {
                ans.getstack(hindexstack, glist);
                int parent = hindexstack.Pop();

                while (hindexstack.Count != 0)
                {
                    int son = hindexstack.Pop();
                    indexq.Enqueue(son);

                    short[] ft = glist[parent].backtracking(glist[son]);
                    map.addmove(ft);
                    parent = son;
                }
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
        public void calStop()
        {
            //Helper.Stop();
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

        Point strpoint = new Point();
        Point timepoint = new Point();
        Point indexpoint = new Point();

        const int plusdot = 100;
        const int dotnum = 3;
        int currentdotnum = 0;
        int count = 0;
        string str = "계산중";
        string timestr = "";
        static string indexstr = "";
        static int movecount = 0;
        int previndex = 0;
        int maxinterval = 0;


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
            strpoint = new Point(270, 150);
            timepoint = new Point(270, 180);
            indexpoint = new Point(550, 180);
            
        }

        public void setSize(Size s)
        {
            strpoint.X = s.Width / 2 - 80;
            strpoint.Y = s.Height / 2 - 80;
            timepoint.X = s.Width / 2 - 80;
            timepoint.Y = s.Height / 2 - 50;

            indexpoint.X = s.Width - 150;
            indexpoint.Y = s.Height - 250;
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
            previndex = 0;
            maxinterval = 0;
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
            int interval = (index - previndex);
            if (interval > maxinterval) maxinterval = interval;
            indexstr = "move : " + ++movecount + "\nindex : " + index + "\ninterval : " + interval
                 ;// + "\nmaxinterval : " + maxinterval;
            previndex = index;
        }



        void drawwait()
        {
            DoubleBuffering.getinstance().getGraphics.DrawString(str, font, brush, strpoint);
        }
        void drawtime()
        {
            DoubleBuffering.getinstance().getGraphics.DrawString(timestr, f, brush, timepoint);
        }
        void drawindexno()
        {
            DoubleBuffering.getinstance().getGraphics.DrawString(indexstr, f, brush, indexpoint);
        }
    }

    class ansHanoi
    {
        Queue<Hanoi> q = new Queue<Hanoi>();
        Hanoilist Glist = new Hanoilist();
        int maxscore = 0;
        bool stop = false;

        public void reset()
        {
            Glist.Clear();
            q.Clear();
            stop = false;
            maxscore = 0;
        }
        public void Stop()
        {
            stop = true;
        }
        void push(Hanoi h, Hanoi parent)
        {
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
        void InsertQueue(Hanoi h, Hanoi parent)
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
            if (!Glist.checkExist(h))
            {
                push(h, parent);
            }
        }

        public Hanoi get_answer(Hanoi h, bool mode)
        {
            Glist.setSize(h.getSize());
            
            push(h, null);

            Hanoi answer = h.answer();

            while(q.Count != 0)
            {
                if (stop) break;
                Hanoi current = q.Dequeue();

                if (current.equal(answer))
                {
                    return current;
                }

                Hanoi clone = current.Clone() as Hanoi;

                if (mode)
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        if (clone.Req_move(i))
                        {
                            push(clone, current);
                            clone = current.Clone() as Hanoi;
                        }
                    }
                }
                else
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        for (int j = 1; j <= 3; j++)
                        {
                            if (i == j)
                            {
                                continue;
                            }

                            if (clone.Req_move(i, j))
                            {
                                InsertQueue(clone, current);
                                clone = current.Clone() as Hanoi;
                            }
                        }
                    }
                }
                    
                
            }

            stop = false;
            return null;
        }

        public Hanoilist getGlist() { return Glist; }
    }
}
