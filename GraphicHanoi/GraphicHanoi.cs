using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using WinFormlib;

namespace GraphicHanoi
{
    class Graphic_object
    {
        internal double x, y;
        internal int width, height;
        protected double draw_x, draw_y;
        protected static Pen edgePen = new Pen(new SolidBrush(Color.Black));
        public virtual void Show()
        {
            DoubleBuffering.getinstance().callback_work += draw;
        }
        protected virtual void draw()
        {

        }
        public virtual void unShow()
        {
            DoubleBuffering.getinstance().callback_work -= draw;
        }

        protected virtual void apply_change()
        {
            draw_x = x - width / 2;
            draw_y = y - height / 2;
        }

        public void setLocation(double x, double y)
        {
            this.x = x;
            this.y = y;
            apply_change();
        }

        public void setSize(int width, int height)
        {
            this.width = width;
            this.height = height;
            apply_change();
        }
    }


    class GraphicMap : Graphic_object
    {
        Queue<int[]> movelist = new Queue<int[]>();
        const int firsthanoiwidth = 40;
        public const int hanoiheight = 24;

        const int upsize = 8;
        const int sidespace = 100;
        const int betweenspace = 9;
        const int pillar_top_space = hanoiheight / 2;
        
        const int space_y = 100;
        const int support_height = 30;

        int hanoisize = 0;
        object moving_locker = new object();
        List<GraphicHanoi> hanoilist = new List<GraphicHanoi>();
        GraphicPillar[] pillars = new GraphicPillar[3];
        GraphicSupp supp = new GraphicSupp();
        
        public GraphicMap()
        {
            x = 0;
            y = 0;
            
            for (int i = 0; i < 3; i++)
            {
                pillars[i] = new GraphicPillar();
            }
            
            GraphicPillar.moveend += delegate ()
            {
                startmove();
            };

            apply_change();
        }

        protected override void apply_change()
        {
            hanoilist.Clear();
            int maxsize = firsthanoiwidth + upsize * hanoisize;
            for (int i = 0; i < 3; i++)
            {
                pillars[i].hanoiclear();
                pillars[i].setSize(firsthanoiwidth / 2, pillar_top_space + hanoisize * hanoiheight);
                pillars[i].setLocation(sidespace + (betweenspace + maxsize) * i + maxsize / 2,
                    space_y + (hanoisize * hanoiheight + pillar_top_space) / 2);
            }
            for (int i = hanoisize - 1; i >= 0; i--)
            {
                GraphicHanoi h = new GraphicHanoi(i + 1);
                h.setSize(firsthanoiwidth + upsize * i, hanoiheight);
                hanoilist.Add(h);
                pillars[0].Push(h);
            }
            supp.setLocation(pillars[1].x, space_y + pillar_top_space + hanoisize * hanoiheight + support_height / 2);
            supp.setSize(betweenspace * 4 + maxsize * 3, support_height);
        }

        public void set(int hanoisize)
        {
            this.hanoisize = hanoisize;
            apply_change();
        }

        public override void Show()
        {
            for(int i = 0;i < 3; i++)
            {
                pillars[i].Show();
            }
            supp.Show();
            for(int i = 0;i < hanoilist.Count; i++)
            {
                hanoilist[i].Show();
            }
        }
        public override void unShow()
        {
            movelist.Clear();
            for(int i =0; i < 3; i++)
            {
                pillars[i].unShow();
            }
            supp.unShow();
            for (int i = 0; i < hanoilist.Count; i++)
            {
                hanoilist[i].unShow();
            }
        }

        public void addmove(int[] ft)
        {
            movelist.Enqueue(ft);
        }

        public delegate void moveevent();
        public static moveevent moveend;
        void paused()
        {


        }
        public void startmove()
        {
            if (Mainprogram.nextmove())
            {
                
                if (movelist.Count != 0)
                {
                    int[] ft = movelist.Dequeue();
                    move(ft[0], ft[1]);
                }
                else
                {
                    if (moveend != null)
                        moveend();
                }
            }
        }

        void move(int from, int to)
        {
            pillars[from - 1].move(pillars[to - 1]);
        }
    }

    class GraphicSupp : Graphic_object
    {
        static Brush brush = new SolidBrush(Color.DarkGray);
        protected override void draw()
        {
            DoubleBuffering.getinstance().getGraphics.FillRectangle(brush, (float)draw_x, (float)draw_y, width, height);
            DoubleBuffering.getinstance().getGraphics.DrawRectangle(edgePen, (float)draw_x, (float)draw_y, width, height);
        }
    }

    class GraphicPillar : Graphic_object
    {
        Stack<GraphicHanoi> hanois = new Stack<GraphicHanoi>();
        static Brush brush = new SolidBrush(Color.DarkGray);
        const int moveover = 50;
        static int motiontime = 15;
        Motion[] ms = new Motion[3];

        public GraphicPillar()
        {
            for(int i = 0;i < 3; i++)
            {
                ms[i] = new Motion();
                ms[i].setmax(motiontime);
                
            }
            ms[0].setnext(ms[1]);
            ms[1].setnext(ms[2]);
            ms[2].setend(end);
        }

        public void hanoiclear()
        {
            hanois.Clear();
        }

        public delegate void moveevent();
        public static moveevent moveend;
        void end()
        {
            if(moveend != null)
            {
                moveend();
            }
        }
        protected override void draw()
        {
            DoubleBuffering.getinstance().getGraphics.FillRectangle(brush, (float)draw_x, (float)draw_y, width, height);
            DoubleBuffering.getinstance().getGraphics.DrawRectangle(edgePen, (float)draw_x, (float)draw_y, width, height);
        }

        public void Push(GraphicHanoi h)
        {
            h.setLocation(this.x, targety());
            hanois.Push(h);
        }
        public double targety()
        {
            return this.draw_y + height - GraphicMap.hanoiheight / 2 - hanois.Count * GraphicMap.hanoiheight;
        }
    
        void settarget(GraphicHanoi h)
        {
            for(int i = 0; i< 3; i++)
            {
                ms[i].settarget(h);
            }
        }

        public void move(GraphicPillar to)
        {
            GraphicHanoi h = hanois.Pop();
            settarget(h);
            int over_y = (int)draw_y - moveover;
            double target_v1 = over_y - h.y;
            ms[0].v_y = target_v1 / motiontime;

            double target_v2 = to.x - x;
            ms[1].v_x = target_v2 / motiontime;

            double target_v3 = to.targety() - over_y;
            ms[2].v_y = target_v3 / motiontime;
            ms[2].setend(() =>
            {
                to.Push(h);
                end();
            });

            ms[0].start();
        }
    }

    class GraphicHanoi : Graphic_object
    {
        static Color color = Color.Brown;
        static Brush brush = new SolidBrush(color);
        static Brush strbrush = new SolidBrush(Color.Black);
        static Font font = new Font("Gulim", 10);
        int num = 0;

        public GraphicHanoi(int num)
        {
            this.num = num;
        }

        protected override void draw()
        {
            DoubleBuffering.getinstance().getGraphics.FillRectangle(brush, (float)draw_x, (float)draw_y, width, height);
            DoubleBuffering.getinstance().getGraphics.DrawRectangle(edgePen, (float)draw_x, (float)draw_y, width, height);
            DoubleBuffering.getinstance().getGraphics.DrawString(num + "", font, strbrush, (float)this.x - 5, (float)this.y - 5);
        }

    }


    class Motion
    {
        Motion nextMotion = null;
        int maxcount = 0;
        int count = 0;
        internal double v_x = 0, v_y = 0;
        Graphic_object target = null;
        Action endAction = null;

        public void settarget(Graphic_object obj)
        {
            target = obj;
        }
        public void setend(Action ac)
        {
            endAction = ac;
        }
        public void setmove(int v_x, int v_y)
        {
            this.v_x = v_x;
            this.v_y = v_y;
        }

        public void setnext(Motion m)
        {
            nextMotion = m;
        }
        public void setmax(int max)
        {
            maxcount = max;
        }
        public void start()
        {
            TextViewer.calc += cal;

        }
        void end()
        {
            TextViewer.calc -= this.cal;
            count = 0;
            if (nextMotion != null)
            {
                nextMotion.start();
            }
            if (endAction != null)
            {
                endAction();
            }
        }

        void cal()
        {
            if(++count > maxcount)
            {
                end();
            }
            else
            {
                target.setLocation(target.x + v_x, target.y + v_y);
            }
        }
    }
}
