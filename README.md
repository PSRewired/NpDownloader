NpDownloader
---
A simple command-line tool to download patch files from Sony in parallel

## Usage
```
Download all patches for the given Title ID

Usage:  download [options] <Title ID>

Arguments:
  Title ID                A comma-separated list of SCE title IDs

Options:
  -o|--outputdir[:<dir>]  The output folder to download patch files. If ending with '/' no titleId folders will be created
  -?|-h|--help            Show help information.
```

### Example
```bash
$> ./NpDownloader download --outputdir=patches ABCD123,DEFG465
```
```
patches/
|___ ABCD123
    |___ Patch1.pkg
    |___ Patch2.pkg
|___ DEFG465
    |___ Patch3.pkg
    |___ Patch4.pkg
    |___ Patch5.pkg
```