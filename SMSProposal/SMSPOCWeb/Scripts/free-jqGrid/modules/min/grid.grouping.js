(function(e){"function"===typeof define&&define.amd?define(["jquery","./grid.base"],e):"object"===typeof exports?e(require("jquery")):e(jQuery)})(function(e){var x=e.jgrid,u=e.fn.jqGrid;x.extend({groupingSetup:function(){return this.each(function(){var b,q;q=this.p;var f=q.colModel,a=q.groupingView,c,l,h=function(){return""};if(null===a||"object"!==typeof a&&!e.isFunction(a))q.grouping=!1;else if(a.groupField.length){void 0===a.visibiltyOnNextGrouping&&(a.visibiltyOnNextGrouping=[]);a.lastvalues=
[];a._locgr||(a.groups=[]);a.counters=[];for(b=0;b<a.groupField.length;b++)a.groupOrder[b]||(a.groupOrder[b]="asc"),a.groupText[b]||(a.groupText[b]="{0}"),"boolean"!==typeof a.groupColumnShow[b]&&(a.groupColumnShow[b]=!0),"boolean"!==typeof a.groupSummary[b]&&(a.groupSummary[b]=!1),a.groupSummaryPos[b]||(a.groupSummaryPos[b]="footer"),c=f[q.iColByName[a.groupField[b]]],!0===a.groupColumnShow[b]?(a.visibiltyOnNextGrouping[b]=!0,null!=c&&!0===c.hidden&&u.showCol.call(e(this),a.groupField[b])):(a.visibiltyOnNextGrouping[b]=
e("#"+x.jqID(q.id+"_"+a.groupField[b])).is(":visible"),null!=c&&!0!==c.hidden&&u.hideCol.call(e(this),a.groupField[b]));a.summary=[];a.hideFirstGroupCol&&(a.formatDisplayField[0]=function(a){return a});b=0;for(q=f.length;b<q;b++)c=f[b],a.hideFirstGroupCol&&!c.hidden&&a.groupField[0]===c.name&&(c.formatter=h),c.summaryType&&(l={nm:c.name,st:c.summaryType,v:"",sr:c.summaryRound,srt:c.summaryRoundType||"round"},c.summaryDivider&&(l.sd=c.summaryDivider,l.vd=""),a.summary.push(l))}else q.grouping=!1})},
groupingPrepare:function(b,q){this.each(function(){var f=this.p.groupingView,a=f.groups,c=f.counters,l=f.lastvalues,h=f.isInTheSameGroup,d=f.groupField.length,r,m,t,v,g,k,x=!1,A=u.groupingCalculations.handler;for(r=0;r<d;r++)if(v=f.groupField[r],m=f.displayField[r],g=b[v],k=null==m?null:b[m],null==k&&(k=g),void 0!==g){t=[];for(m=0;m<=r;m++)t.push(b[f.groupField[m]]);v={idx:r,dataIndex:v,value:g,displayValue:k,startRow:q,cnt:1,keys:t,summary:[]};0===q?(a.push(v),l[r]=g,m={cnt:1,pos:a.length-1,summary:e.extend(!0,
[],f.summary)},c[r]=m):(m={cnt:1,pos:a.length,summary:e.extend(!0,[],f.summary)},"object"===typeof g||(e.isArray(h)&&e.isFunction(h[r])?h[r].call(this,l[r],g,r,f):l[r]===g)?x?(a.push(v),l[r]=g,c[r]=m):(m=c[r],m.cnt+=1,a[m.pos].cnt=m.cnt):(a.push(v),l[r]=g,x=!0,c[r]=m));g=a[m.pos];var w=k=t=void 0;for(t=0;t<m.summary.length;t++)k=m.summary[t],w=e.isArray(k.st)?k.st[v.idx]:k.st,e.isFunction(w)?k.v=w.call(this,k.v,k.nm,b,v):(k.v=A.call(e(this),w,k.v,k.nm,k.sr,k.srt,b),"avg"===w.toLowerCase()&&k.sd&&
(k.vd=A.call(e(this),w,k.vd,k.sd,k.sr,k.srt,b)));g.summary=m.summary}});return this},groupingToggle:function(b,q){this.each(function(){var f=this.p,a=f.groupingView,c=a.minusicon,l=a.plusicon,h=q?e(q).closest("tr.jqgroup"):e("#"+x.jqID(b)),d,r,m=!0,t=!1,v=[],g=function(a){var b,c=a.length;for(b=0;b<c;b++)v.push(a[b])},k=parseInt(h.data("jqgrouplevel"),10);f.frozenColumns&&0<h.length&&(r=h[0].rowIndex,h=e(this.rows[r]),h=h.add(this.grid.fbRows[r]));d=h.find(">td>span.tree-wrap");x.hasAllClasses(d,
c)?(d.removeClass(c).addClass(l),t=!0):d.removeClass(l).addClass(c);for(h=h.next();h.length;h=h.next())if(h.hasClass("jqfoot"))if(d=parseInt(h.data("jqfootlevel"),10),t){if(d=parseInt(h.data("jqfootlevel"),10),(!a.showSummaryOnHide&&d===k||d>k)&&g(h),d<k)break}else{if((d===k||a.showSummaryOnHide&&d===k+1)&&g(h),d<=k)break}else if(h.hasClass("jqgroup"))if(d=parseInt(h.data("jqgrouplevel"),10),t){if(d<=k)break;g(h)}else{if(d<=k)break;d===k+1&&(h.find(">td>span.tree-wrap").removeClass(c).addClass(l),
g(h));m=!1}else(t||m)&&g(h);e(v).css("display",t?"none":"");f.frozenColumns&&e(this).triggerHandler("jqGridResetFrozenHeights",[{header:{resizeDiv:!1,resizedRows:{iRowStart:-1,iRowEnd:-1}},resizeFooter:!1,body:{resizeDiv:!0,resizedRows:{iRowStart:r,iRowEnd:h.length?h[0].rowIndex-1:-1}}}]);this.fixScrollOffsetAndhBoxPadding();e(this).triggerHandler("jqGridGroupingClickGroup",[b,t]);e.isFunction(f.onClickGroup)&&f.onClickGroup.call(this,b,t)});return!1},groupingRender:function(b,q){function f(a,b,f,
h,q){var k=w[a],p,r="",m,d,t,v=0,u,A,E,G=!0;if(0!==b&&0!==w[a].idx)for(p=a;0<=p;p--)if(w[p].idx===w[a].idx-b){k=w[p];break}a=k.cnt;for(u=void 0===q?h:0;u<H;u++){b="&#160;";p=y[u];for(m=0;m<k.summary.length;m++)if(d=k.summary[m],A=e.isArray(d.st)?d.st[f.idx]:d.st,E=e.isArray(p.summaryTpl)?p.summaryTpl[f.idx]:p.summaryTpl||"{0}",d.nm===p.name){"string"===typeof A&&"avg"===A.toLowerCase()&&(d.sd&&d.vd?d.v/=d.vd:d.v&&0<a&&(d.v/=a));try{d.groupCount=k.cnt,d.groupIndex=k.dataIndex,d.groupValue=k.value,
t=c.formatter("",d.v,u,d)}catch(F){t=d.v}b=x.format(E,t);p.summaryFormat&&(b=p.summaryFormat.call(c,f,b,t,p));break}d=m=!1;void 0!==q&&G&&!p.hidden&&(b=q,G=!1,1<h&&(m=!0,v=h-1),d=p.align,p.align="rtl"===l.direction?"right":"left",g.iconColumnName=p.name);0<v&&!p.hidden&&"&#160;"===b?(d&&(p.align=d),v--):(r+="<td role='gridcell' "+c.formatCol(u,1,"")+(m?"colspan='"+h+"'":"")+">"+b+"</td>",d&&(p.align=d))}return r}var a="",c=this[0],l=c.p,h=0,d,r=[],m="",t,v,g=l.groupingView,k=e.makeArray(g.groupSummary),
F=(g.groupCollapse?g.plusicon:g.minusicon)+" tree-wrap",A=g.groupField.length,w=g.groups,y=l.colModel,H=y.length,E=l.page,I=u.getGuiStyles.call(c,"gridRow","jqgroup ui-row-"+l.direction),J=u.getGuiStyles.call(c,"gridRow","jqfoot ui-row-"+l.direction);e.each(y,function(a,b){var c;for(c=0;c<A;c++)if(g.groupField[c]===b.name){r[c]=a;break}});k.reverse();e.each(w,function(u,n){if(g._locgr&&!(n.startRow+n.cnt>(E-1)*q&&n.startRow<E*q))return!0;h++;v=l.id+"ghead_"+n.idx;t=v+"_"+u;m="<span style='cursor:pointer;margin-"+
("rtl"===l.direction?"right:":"left:")+12*n.idx+"px;' class='"+g.commonIconClass+" "+F+"' onclick=\"jQuery('#"+x.jqID(l.id).replace("\\","\\\\")+"').jqGrid('groupingToggle','"+t+"', this);return false;\"></span>";try{e.isArray(g.formatDisplayField)&&e.isFunction(g.formatDisplayField[n.idx])?(n.displayValue=g.formatDisplayField[n.idx].call(c,n.displayValue,n.value,y[r[n.idx]],n.idx,g),d=n.displayValue):d=c.formatter(t,n.displayValue,r[n.idx],n.value)}catch(K){d=n.displayValue}a+="<tr id='"+t+"' data-jqgrouplevel='"+
n.idx+"' "+(g.groupCollapse&&0<n.idx?"style='display:none;' ":"")+"role='row' class='"+I+" "+v+"'>";var z=e.isFunction(g.groupText[n.idx])?g.groupText[n.idx].call(c,d,n.cnt,n.summary):x.template(g.groupText[n.idx],d,n.cnt,n.summary),C=1,D,B,p;p=0;B=A-1===n.idx;"string"!==typeof z&&"number"!==typeof z&&(z=d);"header"===g.groupSummaryPos[n.idx]?(C=1,"cb"!==y[0].name&&"cb"!==y[1].name||C++,"subgrid"!==y[0].name&&"subgrid"!==y[1].name||C++,a+=f(u,0,n,C,m+"<span class='cell-wrapper'>"+z+"</span>")):a+=
"<td role='gridcell' style='padding-left:"+12*n.idx+"px;' colspan='"+H+"'>"+m+z+"</td>";a+="</tr>";if(B){z=w[u+1];B=n.startRow;C=void 0!==z?z.startRow:w[u].startRow+w[u].cnt;g._locgr&&(p=(E-1)*q,p>n.startRow&&(B=p));for(;B<C&&b[B-p];B++)a+=b[B-p].join("");if("header"!==g.groupSummaryPos[n.idx]){if(void 0!==z){for(D=0;D<g.groupField.length&&z.dataIndex!==g.groupField[D];D++);h=g.groupField.length-D}for(p=0;p<h;p++)k[p]&&(a+="<tr data-jqfootlevel='"+(n.idx-p)+(g.groupCollapse&&(0<n.idx-p||!g.showSummaryOnHide)?
"' style='display:none;'":"'")+" role='row' class='"+J+"'>",a+=f(u,p,w[n.idx-p],0),a+="</tr>");h=D}}});this.unbind("jqGridShowHideCol.groupingRender").bind("jqGridShowHideCol.groupingRender",function(){var a=l.iColByName[g.iconColumnName],b,d,f;if(0<=e.inArray("header",g.groupSummaryPos)){for(b=0;b<y.length;b++)if(!y[b].hidden){f=b;break}if(void 0!==f&&a!==f){for(b=0;b<c.rows.length;b++)d=c.rows[b],e(d).hasClass("jqgroup")&&(e(d.cells[f]).html(d.cells[a].innerHTML),e(d.cells[a]).html("&nbsp;"));g.iconColumnName=
y[f].name}}});return a},groupingGroupBy:function(b,q){return this.each(function(){var f=this.p,a=f.groupingView,c,l;"string"===typeof b&&(b=[b]);f.grouping=!0;a._locgr=!1;void 0===a.visibiltyOnNextGrouping&&(a.visibiltyOnNextGrouping=[]);for(c=0;c<a.groupField.length;c++)l=f.colModel[f.iColByName[a.groupField[c]]],!a.groupColumnShow[c]&&a.visibiltyOnNextGrouping[c]&&null!=l&&!0===l.hidden&&u.showCol.call(e(this),a.groupField[c]);for(c=0;c<b.length;c++)a.visibiltyOnNextGrouping[c]=e(f.idSel+"_"+x.jqID(b[c])).is(":visible");
f.groupingView=e.extend(f.groupingView,q||{});a.groupField=b;e(this).trigger("reloadGrid")})},groupingRemove:function(b){return this.each(function(){var q=this.p,f=this.tBodies[0],a=q.groupingView;void 0===b&&(b=!0);q.grouping=!1;if(!0===b){for(q=0;q<a.groupField.length;q++)!a.groupColumnShow[q]&&a.visibiltyOnNextGrouping[q]&&u.showCol.call(e(this),a.groupField);e("tr.jqgroup, tr.jqfoot",f).remove();e("tr.jqgrow",f).filter(":hidden").show()}else e(this).trigger("reloadGrid")})},groupingCalculations:{handler:function(b,
e,f,a,c,l){var h={sum:function(){return parseFloat(e||0)+parseFloat(l[f]||0)},min:function(){return""===e?parseFloat(l[f]||0):Math.min(parseFloat(e),parseFloat(l[f]||0))},max:function(){return""===e?parseFloat(l[f]||0):Math.max(parseFloat(e),parseFloat(l[f]||0))},count:function(){""===e&&(e=0);return l.hasOwnProperty(f)?e+1:0},avg:function(){return h.sum()}};if(!h[b])throw"jqGrid Grouping No such method: "+b;b=h[b]();null!=a&&("fixed"===c?b=b.toFixed(a):(a=Math.pow(10,a),b=Math.round(b*a)/a));return b}}})});
//# sourceMappingURL=grid.grouping.map
