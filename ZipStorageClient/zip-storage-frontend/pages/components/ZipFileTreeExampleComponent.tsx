import FolderTree, { FolderTreeProps, NodeData } from 'react-folder-tree'

function ExampleZipFileTreeComponent() {
    const possibleData : NodeData = {
        name: "<FileName>.zip",
        children: [
          {
            name: "dlls",
            children: [
              {
                name: "<FileName>.dll"
              }
            ]
          },
          {
            name: "images",
            children: [
              {name: "any-png-file.png"},
              {name: "any-jpg-file.jpg"},
              {name: "any-jpeg-file.jpeg"},
            ]
          },
          {
            name: "languages",
            children: [
              {name: "<FileName>_en.xml ('_en' required)"},
              {name: "<FileName>_es.xml"},
            ]
          },
        ]
      }
    return <>
    <FolderTree data={possibleData} showCheckbox={false} readOnly={true}/>
    </>
}

export default ExampleZipFileTreeComponent