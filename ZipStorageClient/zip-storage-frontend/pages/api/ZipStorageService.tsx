import http from "./http-common.js";

interface ListFileItem{
    rootFolderName: string,
    dllsFolder: Array<string>,
    imagesFolder: Array<string>,
    languagesFolder: Array<string>
}

export interface ListFilesResponse{
    files: Array<ListFileItem>
}

class UploadFilesService {
    upload(file:any, onUploadProgress:any) {
      let formData = new FormData();
  
      formData.append("file", file);
  
      return http.post("/v1/zips", formData, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
        onUploadProgress,
      });
    }
  
    getFiles() {
      return http.get("/v1/zips", {
        headers: {
            "Content-Type": "application/json"
        }
      });
    }

    validateZipFile(file:any, onUploadProgress:any){
        let formData = new FormData();
  
        formData.append("file", file);
    
        return http.post("/v1/zips/validate", formData, {
          headers: {
            "Content-Type": "multipart/form-data",
          },
          onUploadProgress,
        });
    }

    deleteFile(fileName:string){
        return http.delete("/v1/zips/"+fileName, {
            headers: {
                "Content-Type": "application/json"
            }
          })
    }
  }
  
  export default new UploadFilesService();