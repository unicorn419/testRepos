import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient, HttpRequest, HttpEventType, HttpResponse } from '@angular/common/http';
import { Console } from '@angular/core/src/console';
import { AutosizeModule } from 'ngx-autosize';
import { forEach } from '@angular/router/src/utils/collection';
declare var $: any;




@Component({
  selector: 'clientinfos',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public progress: number;
  public message: string;
  public ClientInfos: ClientInfo[];

  items;
  templete;

  public DataFileName = "";
  private http: HttpClient;
  private baseUrl: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
    http.get<ClientInfo[]>(baseUrl + 'api/ClientInfoes').subscribe(result => {
      this.ClientInfos = result;
    }, error => console.error(error));

    http.get<string>(baseUrl + 'api/templete').subscribe(result => {

      //this.xmlItems.items = result;
      this.templete =  result;// this.xmlItems.items;
      //result.entries()

    }, error => console.error(error));

  }

  Anaysic(files) {
    var filename;
    for (let file of files) 
      filename = file.name;

    var urlPath = this.baseUrl + 'api/DataElements?datafilename=' + filename;
    this.http.get<XMLItem[]>(urlPath).subscribe(result => {
      this.items = result;
      this.GenerateHtml();
      this.selectSource();
    }, error => console.error(error));
  }

  selectSource()
  {
    for (let i of this.items) {
      var option = new Option(i.xPath, i.name);
      $('#xPath').append(option);
    }
  }

  GenerateHtml()
  {
   
    var $tempDoc = $("#templete");
    $tempDoc.html('');



    var textarea = document.createElement('textarea');
    textarea.innerHTML = this.templete;
    var temp = textarea.innerHTML;

    var selectItems = temp.split('LINE_LINE');
    var idIndex = 0;
    var isInScript = false;
    var scriptText = '';
    for (let tmp of selectItems) {
      var s = tmp.split('SPACE_SPACE').join('&nbsp;');
      //var s = tmp;
      if (s.indexOf("script&") > 0) isInScript = !isInScript;
      if (isInScript) {
        scriptText += s;
        scriptText += '\r\n';
        continue;
      }
      else if (scriptText !== '') {
        scriptText += s;

        var textara = document.createElement('textarea');
        textara.style.width = '100%';
        textara.style.height = '100%';
        //textara.style.ro
        textara.innerHTML = scriptText;
        textara.rows = 8;
        scriptText = '';
        $tempDoc.append(textara);


        continue;
      }

      //fixed item donot need select
      if (s.indexOf('name="ServiceTypeId"') > 0 ||
        s.indexOf('name="DateInserted"') > 0 ||
        s.indexOf('name="IsCurrentData"') > 0)
      {
        $tempDoc.append(s);
        var p = document.createElement('p');
        $tempDoc.append(p);
        continue;
      }

      var i = s.indexOf('source="');
      if (i > 0) {
        $tempDoc.html($tempDoc.html()+s.slice(0, i + 8));
        //$(".templete").html($(".templete").html() + s.slice(0, i + 8));
        s = s.slice(i + 8);
        s = s.slice(s.indexOf('"'));
        var par = document.createElement('font');
        par.className = "myfont";
        var link = document.createElement('button');
        //link.href = '';
        link.className = 'source';
        link.id = "source" + idIndex;
        
        link.innerText = '?';
        //link.click = function () { alert(link.innerText); }
        //link.onclick = this.selectSource;

        par.appendChild(link);
        $tempDoc.append(par);
        //link.addEventListener("click", function cc() { alert("asdf"); });
        //$("myfont").on('click', '.btn', function () { alert("aasdfasdfasdfsdf"); });
       
        //link.click();
        //document.getElementById("source" + idIndex).addEventListener('click',function() { alert("asdf");});
      }
      
      
      i = s.indexOf('default="');
      if (i > 0) {
        $tempDoc.html($tempDoc.html()+ s.slice(0, i + 9));
        s = s.slice(i + 9);
        text = s.substr(0, s.indexOf('"'));
        s = s.slice(s.indexOf('"'));
        s = s.slice(s.indexOf('"'));
        var $par = document.createElement('font');
        var link = document.createElement('button');
        link.className = 'default';
        link.id = "default" + idIndex;

        if (text == '') link.innerText = '?';
        else link.innerText = text;
        $par.appendChild(link);
        //par.append($('#xPath').clone(true));
        $tempDoc.append($par);

        
      }

      i = s.indexOf('isNeedProcess="');
      if (i > 0) {
        $tempDoc.html($tempDoc.html()+ s.slice(0, i + 15));
        s = s.slice(i + 15);
        var text = s.substr(0, s.indexOf('"'));
        s = s.slice(s.indexOf('"'));
        var $par = document.createElement('font');
        var link = document.createElement('button');
        
        link.className = 'isNeedProcess';
        link.id = "isNeedProcess" + idIndex;

        link.innerText = text;
        $par.appendChild(link);
        $tempDoc.append($par);
        idIndex++;
      }

      $tempDoc.html($tempDoc.html() + s);
      var p = document.createElement('p');
      $tempDoc.append(p);

    }
    $('.source').on('click', function () {
      var par = this.parentElement;
      var $button = $(this);
      $button.hide();
      var xpath = $('#xPath');
      var select = document.createElement("select");
      select.id = "xPathSelect";

      var opt = document.createElement("option");
      opt.innerHTML = '';
      opt.disabled = true;
      opt.selected = true;
      opt.value = '';
      select.add(opt);


      opt = document.createElement("option");
      //option.childNodes
      opt.innerText = "No Data";
      opt.value = "";
      select.add(opt);

      for (let i of xpath.children())
      {
        var option = document.createElement("option");
        option.innerText = i.innerHTML;
        option.value = i.value;
        select.add(option);
      }
      select.autofocus = true;
      par.appendChild(select);
      $('#xPathSelect').change(function () {
        var $par = this.parentElement;
        $button.text($("#xPathSelect option:selected").val());
        $button.val( $("#xPathSelect option:selected").text());
        
        this.remove();
        $button.show();
        //$par.append(button);

      });
    })
    $('.default').on('click', function () {
      var $par = this.parentElement;
      var $button = $(this);
      $button.hide();
      var input = document.createElement("input");
      input.id = 'defaultInput';
      
      input.value = $button.text();
      $par.append(input);
      input.autofocus = true;

      $('#defaultInput').keyup(function (event) {
        if (event.keyCode === 13) {
          var $par = this.parentElement;
          $button.text($("#defaultInput").val());
          $button.val($("#defaultInput").val());
          this.remove();
          $button.show();
        }

      });
    })
    $('.isNeedProcess').on('click', function () {
      var $par = $(this).parent();
      var $button = $(this);
      $(this).hide();
      
      var select = document.createElement("select");
      select.className = "isNeedProcessSelect";

      var opt = document.createElement("option");
      opt.innerHTML = '';
      opt.disabled = true;
      opt.selected = true;
      opt.value = '';
      select.add(opt);

      opt = document.createElement("option");
      opt.innerText = "false";
      opt.value = "false";
      select.add(opt);
      opt = document.createElement("option");
      
      opt.innerText = "true";
      opt.value = "true";
      select.add(opt);

      select.autofocus = true;
      $par.append(select);
      $('.isNeedProcessSelect').change(function () {
        var $par = $(this).parent();
        $button.text($(".isNeedProcessSelect option:selected").val());
        $button.val($(".isNeedProcessSelect option:selected").text());
        $button.show();
        $(this).remove();
        $par.append($button);

        if ($button.text() === 'true') {

          alert($par.html());
          var $node = $par.parent();

          var textara = document.createElement('textarea');
          textara.style.width = '100%';
          textara.style.height = '100%';
          //textara.style.ro
          textara.innerHTML = scriptText;
          textara.rows = 8;
          scriptText = '';
          $par.append(textara);
          //node.after(textara);
          alert($par.next().text());
        }
        else
        {
          $par.parentElement.nextSibling.remove();
        }

      });
    })


    
  }

  upload(files) {
    if (files.length === 0)
      return;

    const formData = new FormData();

    for (let file of files)
      formData.append(file.name, file);

    const uploadReq = new HttpRequest('POST', 'api/upload', formData, {
      reportProgress: true,
    });

    this.http.request(uploadReq).subscribe(event => {
      if (event.type === HttpEventType.UploadProgress)
        this.progress = Math.round(100 * event.loaded / event.total);
      else if (event.type === HttpEventType.Response)
      {
        this.message = event.body.toString();
      }
        //this.message = JSON.parse(event.body.toString());
    });
    }
}
 
interface ClientInfo {
  clientInfoId: number;
  clientAliasName: string;
}

interface XMLItem {
  XPath: string;
  IsList: boolean;
  HasValue: boolean;
  Name: string;
}
