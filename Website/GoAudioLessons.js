//-------------------------------------------
// Board
//-------------------------------------------

function GoBoard(container)
{
	//private vars
	var stoneNodes;
	var labelNodes;
	var pointNodes;
	var lineNodesX;
	var lineNodesY;
	var labels;
	var stones;
	var board;
	var size=null;
	var displaySize;
	var innerContainer;

	//Private functions
	
	function createGrid(boardNode)
	{
		var x;
		var y;
		var line;
		for(x=0;x<size;x++)
		{
			line=document.createElement('div');
			line.setAttribute("class", "goal_line");
			lineNodesX[x]=line;
			boardNode.appendChild(line);
		}
		
		for(y=0;y<size;y++)
		{
			line=document.createElement('div');
			line.setAttribute("class", "goal_line");
			lineNodesY[y]=line;
			boardNode.appendChild(line);
		}
	}
	
	function createNodes()
	{
		var x;
		var y;
		var stoneNode;
		var labelNode;
		var pointNode;
		var style;
		var boardNode;
		var labelTextNode;
		innerContainer=document.createElement('div');
		innerContainer.setAttribute("class", "goal_container");
		
		boardNode=document.createElement('div');
		boardNode.setAttribute("class", "goal_board");
		innerContainer.appendChild(boardNode);

		createGrid(boardNode);

		for(y=0;y<size;y++)
			for(x=0;x<size;x++)
			{
				pointNode=document.createElement('div');
				innerContainer.appendChild(pointNode);
				pointNodes[x+"-"+y]=pointNode;
				
				stoneNode=document.createElement('div');
				pointNode.appendChild(stoneNode);
				stoneNode.setAttribute("class", "goal_stone");
				stoneNodes[x+"-"+y]=stoneNode;
				
				labelNode=document.createElement('div');
				pointNode.appendChild(labelNode);
				labelNodes[x+"-"+y]=labelNode;
				labelTextNode=document.createTextNode("");
				labelNode.appendChild(labelTextNode);
			}
		board.clear();
		container.appendChild(innerContainer);
	}

	function checkColor(color)
	{
		if((color!=="-")&&(color!=="B")&&(color!=="W"))
		{
			throw new Error("Invalid Color"+color);
		}
	}
	
	function checkCoordinate(x,y)
	{
		if((x<0)||(y<0)||x>=size||y>=size)
			throw new Error("Invalid Coordinate");
	}
	
	function checkLabel(label)
	{
		return true;
	}
	
	function updateDisplay()
	{
		var pointNode;
	
		displaySize=container.offsetWidth;
		if(container.offsetHeight<container.offsetWidth)
			displaySize=container.offsetHeight;
		else
			displaySize=container.offsetWidth;

		
		innerContainer.style.width=displaySize+"px";
		innerContainer.style.height=displaySize+"px";
		
		for(x=0;x<size;x++)
		{
			line=lineNodesX[x];
			line.style.left=((x+0.5)/size)*displaySize+"px";
			line.style.top=(0.5/size)*displaySize+"px";
			line.style.width="1px";
			line.style.height=(1-1/size)*displaySize+1+"px";
		}
		
		for(y=0;y<size;y++)
		{
			line=lineNodesY[y];
			line.style.left=(0.5/size)*displaySize+"px";
			line.style.top=((y+0.5)/size)*displaySize+"px";
			line.style.width=(1-1/size)*displaySize+1+"px";
			line.style.height="1px";
		}
		
		for(y=0;y<size;y++)
			for(x=0;x<size;x++)
			{
				pointNode=pointNodes[x+"-"+y];
				pointNode.style.left=((x+0)/size)*displaySize+"px";
				pointNode.style.top=((y+0)/size)*displaySize+"px";
				pointNode.style.width=(displaySize/size)+"px";
				pointNode.style.height=(displaySize/size)+"px";
			}
	}

	//public functions
	this.setStone=function(x,y,color)
	{
		checkColor(color);
		checkCoordinate(x,y);
		stones[x+"-"+y]=color;
		if(color=="-")
			pointNodes[x+"-"+y].setAttribute("class", "goal_point goal_empty");
		if(color=="B")
			pointNodes[x+"-"+y].setAttribute("class", "goal_point goal_black");
		if(color=="W")
			pointNodes[x+"-"+y].setAttribute("class", "goal_point goal_white");
	}
	
	this.getStone=function(x,y)
	{
		checkCoordinate(x,y);
		var result=stones[x+"-"+y];
		checkColor(result);
		return result;
	}
	
	this.setLabel=function(x,y,label)
	{
		var symbol=null;
		checkLabel(label);
		checkCoordinate(x,y);
		labels[x+"-"+y]=label;

		if(label=="#CR")
			symbol="circle";
		else if(label=="#TR")
			symbol="triangle";
		else if(label=="#SQ")
			symbol="square";
		else if(label=="#CR")
			symbol="cross";

		if(label=="")
		{
			labelNodes[x+"-"+y].childNodes[0].nodeValue="";
			labelNodes[x+"-"+y].setAttribute("class", "goal_label goal_empty_label");
		}
		else if(symbol!==null)
		{
			labelNodes[x+"-"+y].childNodes[0].nodeValue="";
			labelNodes[x+"-"+y].setAttribute("class", "goal_label goal_used_label goal_symbol_label goal_"+symbol+"_label");
		}
		else
		{
			labelNodes[x+"-"+y].childNodes[0].nodeValue=label;
			labelNodes[x+"-"+y].setAttribute("class", "goal_label goal_used_label goal_text_label");
		}
	}
	
	this.getLabel=function(x,y)
	{
		checkCoordinate(x,y);
		var result=labels[x+"-"+y];
		checkLabel(result);
		return result;
	}
	
	this.clear=function()
	{
		var x;
		var y;
		for(y=0;y<size;y++)
			for(x=0;x<size;x++)
			{
				this.setStone(x,y,"-");
				this.setLabel(x,y,"");
			}
	}
	
	this.getWidth=function()
	{
		return size;
	}
	
	this.getHeight=function()
	{
		return size;
	}
	
	this.setSize=function(width,height)
	{
		if(width!==height)
			throw new Error("Non square boards not supported");
		if(width===size&&height===size)
			return;
		if(container.children.length!=0)
			throw new Error("Container is not empty");
		stoneNodes=new Object();
		labelNodes=new Object();
		pointNodes=new Object();
		labels=new Object();
		stones=new Object();
		lineNodesX=new Object();
		lineNodesY=new Object();
		board=this;
		size=width;
		createNodes();
		updateDisplay();
	}
}

//-------------------------------------------
// Replay
//-------------------------------------------

function GoReplay(board,jsonReplay)
{
	var position=-1;
	var data=JSON.parse(jsonReplay);
	var timeBinding=null;
	var replay=this;

	function checkIndex(index)
	{
		if(index<-1||index>=data.changes.length)
			throw new Error("Invalid Index");
	}
	
	function checkTime(time)
	{
		if(time<0)
			throw new Error("Invalid Time");
	}
	
	function applyDelta(change)
	{
		if(change.a==="L")//Label
			board.setLabel(change.x,change.y,change.v);
		else if(change.a==="S")//Stone
			board.setStone(change.x,change.y,change.v);
		else if(change.a==="Board")//CreateBoard
			board.setSize(change.x,change.y);
		else
			throw new Error("Unknown Action");
	}
	
	function updateBoundTime()
	{
		var time;
		if(typeof(timeBinding)=="object")
			time=timeBinding.currentTime;
		else if(typeof(timeBinding)=="function")
			time=timeBinding();
		else
			throw new Error("Cannot bind to "+typeof(obj));
		if(typeof(time)!="number")
			throw new Error("Time does not evaluate to a number");
		replay.setTimePosition(time);
		setTimeout(updateBoundTime,100);
	}

	this.setIndexPosition=function(index)
	{
		var i;
		checkIndex(index);
		if(index<position)
		{
			position=-1;
			board.clear();
		}
		for(i=position+1;i<=index;i++)
			applyDelta(data.changes[i]);
		position=index;
	}

	this.getIndexPosition=function()
	{
		var result=position;
		checkIndex(result);
		return result;
	}
	
	this.setTimePosition=function(time)
	{
		this.setIndexPosition(this.indexOfTime(time));
	}
	
	this.indexOfTime=function(time)
	{
		var change;
		var i=0;
		for (i in data.changes)
		{
			change=data.changes[i];
			var t=change.t;
			if(((typeof t)=="number")&&(t>time))
			{
				return i-1;
			}
		}
		return i;
	}
	
	this.bindToTime=function(obj)
	{
		if(timeBinding!==null)
			throw new Error("Can bind to time only once");
				timeBinding=obj;
		updateBoundTime();
	}
}