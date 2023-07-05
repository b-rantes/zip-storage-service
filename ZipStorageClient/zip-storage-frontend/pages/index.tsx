import Head from 'next/head'
import Image from 'next/image'
import styles from '../styles/Home.module.css'
import 'react-folder-tree/dist/style.css';
import { useState, useEffect, useRef } from 'react'
import dynamic from 'next/dynamic'
import { NodeData, FolderTreeProps } from 'react-folder-tree'
import UploadFilesService, { ListFilesResponse } from './api/ZipStorageService'
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { ReactConfirmAlertProps, confirmAlert } from 'react-confirm-alert';
import 'react-confirm-alert/src/react-confirm-alert.css';

const ZipFileTreeComponent = dynamic(() => import('./components/ZipFileTreeComponent'), {
  ssr: false
})

const ExampleTreeComponent = dynamic(() => import('./components/ZipFileTreeExampleComponent'), {
  ssr: false
})

const FileUploaderComponent = dynamic(() => import('./components/FileUploadComponent/FileUploadComponent'), {
  ssr: false
})

const zipValidationFailedError = "Zip validation failed: ";
const zipValidationSuccess = "Zip validation success! You are ready to upload it"
const zipUploadFailedError = "File upload failed: "
const zipDeleteFailed = "File delete failed: "

const BasicTree = (props: any) => {
  return (
    <>
      <ExampleTreeComponent/>
    </>
  );
};

export default function Home() {

  function confirmDeleteAlert(fileName:string, yesCb: any, noCb: any){
    const options : ReactConfirmAlertProps = {
      title: 'Delete',
      message: 'Are you sure you want to delete '+fileName+"?",
      buttons: [
        {
          label: 'Yes',
          onClick: yesCb
        },
        {
          label: 'No',
          onClick: noCb
        }
      ],
      closeOnEscape: false,
      closeOnClickOutside: false,
      overlayClassName: "overlay-custom-class-name"
    };

    confirmAlert(options);
  }

  
  
  
  const toastDefaultProps:any = {
    position: "bottom-right",
    autoClose: 5000,
    hideProgressBar: false,
    closeOnClick: true,
    pauseOnHover: true,
    draggable: true,
    progress: undefined,
    theme: "colored",
    }

  const [sendBtnEnabled, setSendBtn] = useState<boolean | null>(false)
  const [selectedFile, setFile] = useState<any>(null);

  function MountedTree(){
    const [data, setData] = useState<ListFilesResponse | null>(null)
    const [isLoading, setLoading] = useState(false)

    function deleteFile(fileName: string){
      UploadFilesService.deleteFile(fileName)
        .then(res => {
          window.location.reload();
        }).catch(err => {
          toast.error(zipDeleteFailed+err.response.data.error, toastDefaultProps)
        })
    }
  
    useEffect(() => {
      setLoading(true);
      UploadFilesService.getFiles()
        .then((res) => {
          let response:ListFilesResponse = {
            files: res.data
          };
          setData(response)
          setLoading(false)
        })
        .catch((error) => {
          console.log("error", error);
          setData(null);
          setLoading(false);
        });
    }, [])
  
    if(isLoading) return <p>Loading...</p>
    if (!data) return <p>Error fetching service</p>
  
    let folders:Array<NodeData> = []
  
    for (const iterator of data.files) {
      let dllsFiles: NodeData[] = [];
      iterator.dllsFolder.forEach(dllFileName => dllsFiles.push({name: dllFileName, isOpen: false}))
      
      let imagesFiles: NodeData[] = [];
      iterator.imagesFolder.forEach(imageFileName => imagesFiles.push({name: imageFileName, isOpen: false}))
  
      let languagesFiles: NodeData[] = [];
      iterator.languagesFolder.forEach(languageFileName => languagesFiles.push({name: languageFileName, isOpen: false}))
  
      let file:NodeData = {
        name: iterator.rootFolderName,
        isOpen: false,
        children: [
          {name: "dlls", children: dllsFiles},
          {name: "images", children: imagesFiles},
          {name: "languages", children: languagesFiles}
        ]
      };
  
      folders.push(file);
    }
    
    return (
      <>
        <div>
          {folders.map((folder) => {
            return <ZipFileTreeComponent props={{readOnly: true, showCheckbox:false, data:folder}} deleteCb={
              () => confirmDeleteAlert(folder.name, () => deleteFile(folder.name), () => {})} ></ZipFileTreeComponent>
          })}
        </div>
      </>
    )
  }

  function SendRequest(){
    console.log("file", selectedFile);
    UploadFilesService.upload(selectedFile, (event:any) => {
      console.log("loaded", event.loaded);
      console.log("total", event.total);
    }).then(res => {
      window.location.reload();
    })
    .catch(err => {
      toast.error(zipUploadFailedError+err.response.data.error, toastDefaultProps);
    });
  }

  function ValidateFile(file: File[]){
    if(file.length == 0){
      setSendBtn(false);
      setFile(null);
      return;
    }
    UploadFilesService.validateZipFile(file[0], (event:any) => {
      console.log("loaded", event.loaded);
      console.log("total", event.total);
    }).then(res => {
      setSendBtn(true);
      setFile(file[0])
      toast.success(zipValidationSuccess, toastDefaultProps);
    })
    .catch((err) => {
      setSendBtn(false);
      setFile(null);
      toast.error(zipValidationFailedError+err.response.data.error, toastDefaultProps);
    });
  }

  return (
    <div className={styles.container}>
      <Head>
        <title>Zip Storage</title>
        <link rel="icon" href="/favicon.ico" />
      </Head>

      <main className={styles.main}>
        <h1 className={styles.title}>
          Zip File Storage
        </h1>

        <p className={styles.description}>
          Upload your zip file with the expected structure as below
        </p>
        <BasicTree/>
      </main>

      <div className={styles.fileBtn}>
        <FileUploaderComponent accept=".zip" label={""} updateFilesCb={ValidateFile}></FileUploaderComponent>
        <button disabled={!sendBtnEnabled} className={styles.submitBtn} type="button" onClick={SendRequest}>Upload</button>      
      </div>

      <div>
        <h3>Uploaded files</h3>
        <MountedTree/>
      </div>
      <footer className={styles.footer}>
        <a
          href="https://github.com/b-rantes/zip-storage-service"
          target="_blank"
          rel="noopener noreferrer"
        >
          Global Games - Game Storage System
        </a>
      </footer>
      <ToastContainer />
    </div>
  )
}
