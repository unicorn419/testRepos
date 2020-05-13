import { Component, Inject,OnInit } from '@angular/core';
import { HttpClient, HttpRequest, HttpEventType, HttpResponse  } from '@angular/common/http';
import { forEach } from '@angular/router/src/utils/collection';

@Component({
  selector: 'app-counter-component',
  templateUrl: './counter.component.html'
})

export class CounterComponent implements OnInit {



  public XMLConfigData: XMLConfigDataItemModel[];
  public ConfigurationModelData: ConfigurationModel;
  public SubService: object[];
  public CurrentSubServiceSelected: string;

  public CurrentService: object;
  public CurrentItem: object;
  public IsRootService: boolean;
  public RootServiceIndex: number;
  public SubServiceIndex: number;


  public CurrentConvertFunc: string;
  public CurrentConvertParamater: string;

  public convertFunc = ['', 'StringCompare', 'StringReplace', 'PostalFormat', 'DateFormat', 'MonthToYear', 'YearToMonth','Today','Now'];
  public trueFalseOptions = ['false', 'true'];
  public fileEncoding = ['ISO8859-1', 'IBM437'];
  public fileExtension = ['.xml', '.uif', '.xif', '.txt', '.csv'];

  private http: HttpClient;
  private baseUrl: string;

  private clientinfoId: number;

  public progress: number;
  public message: string;



  


  ngOnInit() {
    //this.ClientId = 123;
    //console.debug(this.XMLConfigData);
  }



  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
    this.IsRootService = true;
    this.clientinfoId = 1;
    this.GetTemplate();
    this.GetSubService();
   

  }
  upload(files) {
    if (files.length === 0)
      return;

    const formData = new FormData();

    for (let file of files)
      formData.append(file.name, file);

    
    const uploadReq = new HttpRequest('POST', this.baseUrl+ 'api/Upload', formData, {
      reportProgress: true,
    });

    this.http.request(uploadReq).subscribe(event => {
      if (event.type === HttpEventType.UploadProgress) {
        this.progress = Math.round(100 * event.loaded / event.total);
        if (this.progress == 100) {

        }
      }
      else if (event.type === HttpEventType.Response) {
        this.message = event.body.toString();
        this.GetDataItem(files[0]["name"]);
      }
    });
  }


  Submit() {
    this.ConfigurationModelData.clientInfoId = this.clientinfoId;
    this.http.post<ConfigurationModel>(this.baseUrl + 'api/DataItem/UpdateConfiguration', this.ConfigurationModelData).subscribe(result => {
      //this.ConfigurationModelData = result;
      alert("OK");
    }, error => console.error(error));
  }

  GetDataItem(fileName: string)
  {
    this.XMLConfigData = null;
    if (fileName.toLowerCase().indexOf(".xml") > 0 || fileName.toLowerCase().indexOf(".xif") > 0) {
      this.http.get<XMLConfigDataItemModel[]>(this.baseUrl + 'api/DataItem/GetXMLElement?datafile=' + fileName).subscribe(result => {
        this.XMLConfigData = result;
      }, error => console.error(error));
      return;
    }
    
    if (fileName.indexOf(".uif") > 0) {
      this.http.get<XMLConfigDataItemModel[]>(this.baseUrl + 'api/DataItem/GetUIFElement?datafile=' + fileName).subscribe(result => {
        this.XMLConfigData = result;
      }, error => console.error(error));
      return;
    }

  }

  GetSubService() {
    this.http.get<object[]>(this.baseUrl + 'api/DataItem/GetSubServices').subscribe(result => {
      this.SubService = result;

    }, error => console.error(error));
  }


  GetTemplate()
  {
    let extension = ".xif";
    if (this.ConfigurationModelData != null) extension = this.ConfigurationModelData["fileExtension"];
    this.http.get<ConfigurationModel>(this.baseUrl + 'api/DataItem/GetTemplate?clientInfoId=' + this.clientinfoId + '&fileExtension=' + extension).subscribe(result => {
      this.ConfigurationModelData = result;
      if (this.ConfigurationModelData)
        this.CurrentService = this.ConfigurationModelData.services[0];
      this.CurrentItem = this.CurrentService["configItems"][0]
      if (this.CurrentItem["convertFunc"] != null && this.CurrentItem["convertFunc"] != '') {
        this.CurrentConvertFunc = this.CurrentItem["convertFunc"].split(';')[0];
        this.CurrentConvertParamater = this.CurrentItem["convertFunc"].split(';')[0];

      }
    }, error => console.error(error));

    

  }

  DisplayService(i, j) {
    if (j < 0) {
      this.CurrentService = this.ConfigurationModelData["services"][i];
      this.IsRootService = true;
    }
    else {
      this.CurrentService = this.ConfigurationModelData["services"][i]["services"][j];
      this.IsRootService = false;
    }
    this.RootServiceIndex = i;
    this.SubServiceIndex = j;
    this.CurrentItem = this.CurrentService["configItems"][0];
  }

  DisplayItems(itemName) {
    for (let key in this.CurrentService["configItems"]) {
      let val = this.CurrentService["configItems"][key];
      if (val['name'] == itemName) {
        this.CurrentItem = val;
        break;
      }
    }
    if (this.CurrentItem["convertFunc"] != null && this.CurrentItem["convertFunc"] != '') {
      this.CurrentConvertFunc = this.CurrentItem["convertFunc"].split(';')[0];
      this.CurrentConvertParamater = this.CurrentItem["convertFunc"].split(';')[1];

    }
    else {
    this.CurrentConvertFunc = '';
      this.CurrentConvertParamater = '';}
  }

  RemoveConfigItem(name) {
    for (let key in this.CurrentService["configItems"])
    {
      let val = this.CurrentService["configItems"][key];
      if (val['name'] == name) {
        val["isUpdated"] = false;
        break;
      }
    }
  }

  AddConfigItem() {
    this.CurrentItem["isUpdated"] = true;
  }

  ConvertFuncChange() {
    this.CurrentItem["convertFunc"] = this.CurrentConvertFunc + ";" + this.CurrentConvertParamater;
  }


  AddSubService() {

    let obj;
    for (let key in this.SubService) {
      let val = this.SubService[key];
      if (val["name"] == this.CurrentSubServiceSelected) {
        obj = JSON.parse(JSON.stringify(val));
        break;
      }
    }

    this.ConfigurationModelData["services"][this.RootServiceIndex]["services"].push(obj);

  }
  RemoveSubService() {
    this.ConfigurationModelData["services"][this.RootServiceIndex]["services"].splice(this.SubServiceIndex, 1);
    if (this.SubServiceIndex >= this.ConfigurationModelData["services"][this.RootServiceIndex]["services"].length)
    {
      this.SubServiceIndex--;
    }

    this.DisplayService(this.RootServiceIndex, this.SubServiceIndex);
  }
  
  
}

interface XMLConfigDataItemModel {
  Name: string;
  xPath: string;
  IsList: boolean;
  HasValue: boolean;
}

interface ConfigurationModel {
  services: object[];
  clientInfoId: number;
  isEnabled: boolean;
  fileExtension: string;
  fileEncoding: string;
  parserAssembly: string;
  parserClassName: string;
}

interface ServiceModel {
  isUpdated: boolean;

  serviceType: string;
  source: string;

  name: string;

  configItems: object[];
  services: object[];

}
